# BigBlueButton Video Downloader

BigBlueButton is an online conference system and has features such as conference recording and presentation.
BigBlueButton Downloader allows you to download Presentation and Video at BBB Playback.

## Getting Started

- Download BigBlueButton Downloader compatible with your computer's operating system.
- Add the BigBlueButton Downloader folder to the environment variables.


![enter image description here](https://i.ibb.co/djX2HQS/bbb-downloader.png)

## Single Download

![enter image description here](https://i.ibb.co/GpfKQD6/bbb-single-download.png)

Single Download is used to download only one video.

### Options
------

1) -m: Enables Multi Thread processing.
2) -u: Conference playback url
3) -o: Output filename
4) -d: Output Folder Address (Full Path)
5) -w: Determines whether the camera image is downloaded. Get true or false. (If false is given, there will be no sound in the images.)
6) -v: Determines whether the screen recording video is downloaded. Get true or false.
7) -p: Determines whether the presentation will be downloaded. Get true or false.

### Example Usage
```bash
 ./BigBlueButton-Video-Downloader \
 -m true \
 -u https://playback.url \
 -o outputFileName \
 -d /Users/berkay/Desktop/TestDirectory/ \
 -w true \
 -v true \ 
 -p true

```

   ## Batch Download

![enter image description here](https://i.ibb.co/8NXGCMD/bbb-batch-download.png)   

Batch Download is used to download multiple videos in parallel.

## Options

1) -m: Enables Multi Thread processing.
2) -d: Output Folder Address (Full Path)
3) -f: BatchFile File path

The BatchFile File must be a text file in the following format.

## BatchFile.Txt Format

    playbackUrl|outputFileName|isDownloadDeskshare|isDownloadPresentation|isDownloadWebcam
   
### Example BatchFile

    somePlayBackUrl1|file1|true|true|true
    somePlayBackUrl2|file2|false|true|true
    somePlaybackUrl3|file3|true|false|true



## ❤️&nbsp; Community and Contributions

The BigBlueButton Video Downloader is a **community-driven open source project**. We are committed to a fully transparent development process and **highly appreciate any contributions**. Whether you are helping us fixing bugs, proposing new feature, improving our documentation or spreading the word - **we would love to have you as part of the BBB Video Downloader community**.



## 🤝&nbsp; Found a bug? Missing a specific feature?

Feel free to **file a new issue** with a respective title and description on the the [berkayalcin/BigBlueButton-Video-Downloader](https://github.com/berkayalcin/BigBlueButton-Video-Downloader/issues) repository. If you already found a solution to your problem, **we would love to review your pull request**!



## 📘&nbsp; License
The BigBlueButton Video Downloader is released under the under terms of the [MIT License](LICENSE).
