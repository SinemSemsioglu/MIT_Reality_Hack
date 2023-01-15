using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Dolby.Millicast;
using System.Collections.Generic;
using System.Linq;

public class VideoConfigExample : MonoBehaviour
{
  private McPublisher _publisher;
  private McSubscriber _subscriber;

  [SerializeField] private Camera cam;
  [SerializeField] private AudioSource audioSource;

  [SerializeField] private Credentials publishCredentials;
  [SerializeField] private Credentials subscribeCredentials;


  [SerializeField] private RawImage subscribeImage;
  [SerializeField] private RawImage sourceImage;

  // This is the receiving audio source.
  [SerializeField] private AudioSource subscribeAudioSource;

  [SerializeField] private Button publishButton;
  [SerializeField] private Button subscribeButton;

  [SerializeField] private Transform rotateObject;


  private readonly VideoConfig _videoConfig = new VideoConfig();
  private StreamSize _streamSize = null;

  // Values for UI dropdownds
  private Dictionary<string, uint> bandwidthOptions =
  new Dictionary<string, uint>()
  {
            { "Not set", 0 },
            { "10000", 10000 },
            { "6000", 6000 },
            { "2000", 2000 },
            { "1000", 1000 },
            { "500",  500 },
            { "125",  125 },
  };

  private Dictionary<string, double> scaleResolutionDownOptions =
      new Dictionary<string, double>()
  {
        { "Not scaling", 1.0f },
        { "Down scale by 2.0", 2.0f },
        { "Down scale by 4.0", 4.0f },
        { "Down scale by 8.0", 8.0f },
        { "Down scale by 16.0", 16.0f }
  };


  private Dictionary<string, StreamSize> publishResolution =
      new Dictionary<string, StreamSize>()
  {
        { "1280 x 720 (720p)", new StreamSize { width = 1280, height = 720 } },
        { "1920 x 1080 (1080p)", new StreamSize { width = 1920, height = 1080 } },
        { "2560 x 1440 (1440p)", new StreamSize { width = 2560, height = 1440 } },
        { "2048 x 1080 (2K)", new StreamSize { width = 2048, height = 1080 } },
        { "3840 x 2160 (4K)", new StreamSize { width = 3840, height = 2160} },
  };

  private Dictionary<string, uint> framerateOptions =
      new Dictionary<string, uint>
  {
        { "Not set", 0},
        { "60", 60 },
        { "30", 30 },
        { "20", 20 },
        { "10", 10 },
        { "5", 5 },
        { "0", 0 },
  };

  private Dictionary<string, VideoCodec> videoCodecOptions =
  new Dictionary<string, VideoCodec>
  {
        { "VP8", VideoCodec.VP8 },
        { "VP9", VideoCodec.VP9 },
        { "H264", VideoCodec.H264 },
        { "AV1", VideoCodec.AV1 }
  };

  // UI Settings for video configuration
  [SerializeField] private TMP_Dropdown maxBitrateSelector;
  [SerializeField] private TMP_Dropdown minBitrateSelector;
  [SerializeField] private TMP_Dropdown maxFramerateSelector;
  [SerializeField] private TMP_Dropdown resolutionDownScalingSelector;
  [SerializeField] private TMP_Dropdown publishResolutionSelector;

  [SerializeField] private TMP_Dropdown videoCodecSelector;
  [SerializeField] private Button updateSettingsButton;

  void Awake()
  {
    // Setting up dropdowns and buttons
    publishButton.onClick.AddListener(Publish);
    subscribeButton.onClick.AddListener(Subscribe);
    updateSettingsButton.onClick.AddListener(CommitVideoConfigChange);

    publishButton.interactable = true;
    subscribeButton.interactable = false;

    maxBitrateSelector.options = bandwidthOptions
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    maxBitrateSelector.onValueChanged.AddListener(ChangeMaxBitrate);

    minBitrateSelector.options = bandwidthOptions
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    minBitrateSelector.onValueChanged.AddListener(ChangeMinBitrate);

    resolutionDownScalingSelector.options = scaleResolutionDownOptions
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    resolutionDownScalingSelector.onValueChanged.AddListener(ChangeResolutionDownScaling);

    maxFramerateSelector.options = framerateOptions
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    maxFramerateSelector.onValueChanged.AddListener(ChangeMaxFramerate);

    publishResolutionSelector.options = publishResolution
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    publishResolutionSelector.onValueChanged.AddListener(ChangePublishingResolution);

    videoCodecSelector.options = videoCodecOptions
        .Select(pair => new TMP_Dropdown.OptionData { text = pair.Key })
        .ToList();
    videoCodecSelector.onValueChanged.AddListener(ChangeVideoCodec);
  }

  void Start()
  {
    // Setting default text on startup
    maxBitrateSelector.captionText.text = "Max Bitrate (Kbps)";
    minBitrateSelector.captionText.text = "Min Bitrate (Kbps)";
    maxFramerateSelector.captionText.text = "Max Framerate";
    resolutionDownScalingSelector.captionText.text = "Resolution Scaledown";
    publishResolutionSelector.captionText.text = "Publish Resolution";
    videoCodecSelector.captionText.text = "Video Codec";

    if (cam == null || audioSource == null)
    {
      Debug.Log("Must create Camera and AudioSource");
      return;
    }

    if (audioSource.clip != null)
    {
      audioSource.loop = true;
    }

    _publisher = gameObject.AddComponent<McPublisher>();
    _publisher.credentials = publishCredentials;

    _publisher.options.dtx = true;
    _publisher.options.stereo = true;

    // Not necessary to implement those callbacks.
    _publisher.OnPublishing += (publisher) =>
    {
      Debug.Log($"{publisher} is publishing!");

      // Subscriber setup.
      if (_subscriber == null)
      {
        _subscriber = gameObject.AddComponent<McSubscriber>();
        _subscriber.credentials = subscribeCredentials;
        _subscriber.AddRenderImage(subscribeImage);
        _subscriber.AddRenderAudioSource(subscribeAudioSource);
      }

      // Setup the buttons for unpublishing and subcribing
      publishButton.onClick.RemoveAllListeners();
      publishButton.onClick.AddListener(UnPublish);
      publishButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
      publishButton.interactable = true;
      subscribeButton.interactable = true;
    };

    _publisher.OnViewerCount += (publisher, count) =>
    {
      Debug.Log($"{publisher} viewer count is currently: {count}");
    };
  }

  void Subscribe()
  {
    subscribeButton.interactable = false;
    _subscriber.Subscribe();
  }

  void Publish()
  {
    if (!audioSource.isPlaying)
    {
      audioSource.Play();
    }

    _publisher.SetAudioSource(audioSource);
    _publisher.SetVideoSource(cam, _streamSize);
    _publisher.AddRenderImage(sourceImage);

    publishButton.interactable = false;
    videoCodecSelector.interactable = false;
    _publisher.Publish();
  }

  void UnPublish()
  {
    publishButton.onClick.RemoveAllListeners();
    publishButton.onClick.AddListener(Publish);
    publishButton.GetComponentInChildren<TextMeshProUGUI>().text = "Publish";
    _publisher.UnPublish();
    videoCodecSelector.interactable = true;
    subscribeButton.interactable = false;
  }


  void ChangeMaxBitrate(int index)
  {
    _videoConfig.maxBitrate = bandwidthOptions.Values.ElementAt(index);
  }

  void ChangeMinBitrate(int index)
  {
    _videoConfig.minBitrate = bandwidthOptions.Values.ElementAt(index);

  }

  void ChangeMaxFramerate(int index)
  {
    _videoConfig.maxFramerate = framerateOptions.Values.ElementAt(index);
  }


  void ChangeResolutionDownScaling(int index)
  {
    _videoConfig.resolutionDownScaling = scaleResolutionDownOptions.Values.ElementAt(index);
  }

  void ChangeVideoCodec(int index)
  {
    var codec = videoCodecOptions.Values.ElementAt(index);
    _publisher.options.videoCodec = codec;

    var options = new List<TMP_Dropdown.OptionData>();
    options.Add(new TMP_Dropdown.OptionData { text = "1280 x 720 (720p)" });

    Capabilities.SupportedResolutions maxRes = Capabilities.GetMaximumSupportedResolution(codec);

    if (maxRes > Capabilities.SupportedResolutions.RES_1080P)
    {
      options.Add(new TMP_Dropdown.OptionData { text = "1920 x 1080 (1080p)" });
    }

    if (maxRes > Capabilities.SupportedResolutions.RES_1440P)
    {
      options.Add(new TMP_Dropdown.OptionData { text = "2560 x 1440 (1440p)" });
    }

    if (maxRes > Capabilities.SupportedResolutions.RES_2K)
    {
      options.Add(new TMP_Dropdown.OptionData { text = "2048 x 1080 (2K)" });
    }

    if (maxRes > Capabilities.SupportedResolutions.RES_4K)
    {
      options.Add(new TMP_Dropdown.OptionData { text = "3840 x 2160 (4K)" });
    }
    publishResolutionSelector.options = options;
  }


  void ChangePublishingResolution(int index)
  {
    var key = publishResolutionSelector.options.ElementAt(index).text;
    _streamSize = publishResolution[key];
    _publisher.SetVideoSource(cam, _streamSize);
  }

  void CommitVideoConfigChange()
  {
    _publisher.SetVideoConfig(_videoConfig);
  }

  // Update is called once per frame
  void Update()
  {
    if (rotateObject != null)
    {
      float t = Time.deltaTime;
      rotateObject.Rotate(100 * t, 200 * t, 300 * t);
    }
  }

  /// <summary>
  /// <c>Millicast.Destroy</c> must be called here.
  /// </summary>
  void OnDestroy()
  {
  }
}


