<p align="right"><a href="./README.md">简体中文</a></p>

<p align="center">
  <img src="./assets/readme-banner.svg" alt="LaPian Pro Banner" width="100%" />
</p>

<p align="center">
  <img src="./assets/logo.svg" alt="LaPian Pro Logo" width="96" />
</p>

<h1 align="center">LaPian Pro</h1>

<p align="center">A local video shot-analysis annotation tool built with C#, designed to capture structured shot notes, key frames, and scene analysis.</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-.NET%20Framework%204.8.1-512BD4?style=for-the-badge&logo=csharp&logoColor=white" alt=".NET Framework 4.8.1" />
  <img src="https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=for-the-badge&logo=visualstudio&logoColor=white" alt="Visual Studio 2022" />
  <img src="https://img.shields.io/badge/Platform-Windows-0078D4?style=for-the-badge&logo=windows11&logoColor=white" alt="Windows" />
  <img src="https://img.shields.io/badge/Data-JSON-0A7E8C?style=for-the-badge&logo=json&logoColor=white" alt="JSON" />
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Scene-Video%20Analysis-0F766E?style=flat-square" alt="Video Analysis" />
  <img src="https://img.shields.io/badge/Mode-Local%20First-E67E22?style=flat-square" alt="Local First" />
  <img src="https://img.shields.io/badge/UI-Browser%20Based-1D4ED8?style=flat-square" alt="Browser UI" />
  <img src="https://img.shields.io/badge/Storage-Structured%20Notes-334155?style=flat-square" alt="Structured Notes" />
</p>

## Overview

LaPian Pro is a local tool for analyzing short videos, commercials, films, and reference footage shot by shot. Once started, it launches a lightweight local service and opens a browser-based interface where you can import a video, tag shot type and camera movement, write analysis notes, capture key frames, and save everything as JSON grouped by video.

It is intended to be a practical, lightweight workstation for solo study and reference breakdowns rather than a heavy media management system.

## Interface

<p align="center">
  <img src="./assets/demo.png" alt="LaPian Pro interface screenshot" width="100%" />
</p>

<p align="center">Video preview on the left, annotation form in the center, and saved shot records on the right.</p>

## Highlights

- Runs locally without external services, making it suitable for long-term personal archives.
- Lets you annotate shot type, camera movement, and written analysis around exact timestamps.
- Captures the current frame automatically to reduce manual screenshot work.
- Stores annotations as structured JSON files, which makes later processing easier.
- Works well both as a personal shot-analysis tool and as a prototype base for future export features.

## Who It Is For

- People studying short-form videos, commercials, and films through shot breakdowns
- People who want to record visual analysis in a structured way
- People building their own reusable reference library and analysis archive

## Current Features

- Starts a lightweight local service and opens the browser automatically
- Imports local video files for shot-by-shot review
- Records shot type, camera movement, and free-form analysis
- Displays the current timestamp and captures frame thumbnails
- Saves annotations by video-specific identifier
- Supports viewing, loading back, and deleting existing annotations
- Persists data in JSON for later processing

## Tech Stack

- C#
- .NET Framework 4.8.1
- `HttpListener`
- Native HTML / CSS / JavaScript
- `Newtonsoft.Json`

## How To Run

1. Open `拉片pro.sln` in Visual Studio 2022
2. Restore NuGet packages
3. Build and run in `Debug` or `Release`
4. The program listens on `http://localhost:8848/`
5. After the browser opens, choose a local video file and start annotating

## Data Storage

- Annotation data is stored per video
- Default path: `case/<video-id>/index.json`
- Each record includes shot type, movement, analysis text, timestamp, frame image, and creation time

Note:
The UI currently uses the field name `videoMd5`, but the current implementation actually uses a local unique ID generated from the file name, size, and modified time. It is not a real MD5 hash.

## Recommended GitHub Description

You can use this as the repository description in the GitHub `About` section:

`A local video shot-analysis annotation tool built with C#, featuring shot tags, movement notes, frame capture, and JSON persistence.`

## Recommended GitHub Topics

These are not README badges. Add them manually in `About -> Settings` on GitHub:

`csharp` `dotnet-framework` `visual-studio-2022` `video-analysis` `annotation-tool` `film-study` `json` `windows`

## Possible Next Steps

- Move the front-end page into the formal source tree
- Separate development data from sample data
- Add export formats such as Markdown / Excel / CSV
- Add more classification fields such as composition, mood, sound, and transitions
- Support timeline-based search and filtering
