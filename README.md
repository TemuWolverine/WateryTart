# WateryTart, a MediaAssistant Client
> Disclaimer: This is an unofficial project and is not affiliated with, endorsed by, or associated with the Music Assistant project.
> It is really just a proof of concept that has gotten out of hand - I'm very much a hobbyist coder, so I won't be surprised if I haven't somehow invented the definitive example of 'Worst Practices'


## Goals
This is aimed as a Plexamp-like experience for MediaAssistant, with a personal focus on Windows and Linux/RaspberryPi on a 5" touch screen.

While there is an Android project, this is mostly provided through Avalonia's crossplatform project setup and is currently not actively being tested/developed for Android.


## Download & Install

### Requirements
- [.NET 10.0 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0)

### Options
* Windows x64
* Linux x64
* Linux ARM x64 ie for Raspberry Pi 5

## Platform specific
Linux ARM x64 can take advantage of GPIO pins for rotary encoders for volume control. This currently uses PINs 17 and 27, with 20 pulses per rotation. Eventually this will all be configurable.

## Whats with the name?
WateryTart is uses Avalonia, which sounds like it comes from Camelot, and "You can't expect to wield supreme executive power just 'cause some watery tart threw a sword at you!".

## License

MIT License — see [LICENSE](LICENSE) for details.