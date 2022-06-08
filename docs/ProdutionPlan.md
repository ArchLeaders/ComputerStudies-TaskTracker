# Prodution Plan

This document covers a general plan for creating Task Tracker.

## Tools and Resources

The tools and resources used in development.

### Development Tools

- Visual Studio (DotNET Development)
- Visual Studio Code (Python/Web Development)

### Technoligies

- GitHub (Code repository, workflows, and web hosting)
- Heroku (Bot hosting)
- Discord (Receiving server/service)

### Materials

- Computer & peripherals, etc

## Production steps

Each step in production is briefly explained in this section.

### Discord Bot Logic (Python)

- Setup a basic working bot that is syncronized with Discord.
  - Status: `Done` | Commit: [`0a92a04`](https://github.com/ArchLeaders/TaskTracker/commit/0a92a0409da2c05a99109cef6a97fde4b75e17d2)

- Implement registering a new server.
  - Status: `Done` | Commit: [`503666f`](https://github.com/ArchLeaders/TaskTracker/commit/503666f9f23b7d3519bb647075f7a3c95276a597) -> [`4d83311`](https://github.com/ArchLeaders/TaskTracker/commit/4d83311dbd042c2dac87dfc24e7ec7252f6f624e)

- Prototype task iteration (without data).
  - Status: `Done` | Commit: [`17b74d4`](https://github.com/ArchLeaders/TaskTracker/commit/17b74d4ffece761a7d9ba06aeb191c93b60472bd)

- Add runtime edit/misc command(s).
  - Status: `Done` | Commit: [`5ee8f89`](https://github.com/ArchLeaders/TaskTracker/commit/5ee8f894bfbaf3b7d7bbcebcc0d519064d4638e9)

### Setup Wizard (C#/XAML)

- Create a basic GUI from the Stylet WPF template.
  - Status: `Done` | Commit: [`345a210`](https://github.com/ArchLeaders/TaskTracker/commit/345a210182eaafbe19adfcba48a687f7ff3b1b96) 

- Setup Task and Var View/ViewModel editors.
  - Status: `Done` | Commit: [`4069d91`](https://github.com/ArchLeaders/TaskTracker/commit/4069d917a64e42ad9703730ca5f693b39b41ebd2) & [`83e0325`](https://github.com/ArchLeaders/TaskTracker/commit/83e0325cfc1941d8420facec72c26acbf55d093a)

- Finalize GUI design.
  - Status: `Done` | Commit: [`0952d5a`](https://github.com/ArchLeaders/TaskTracker/commit/0952d5a3e8f7971a55c692caf533fa68474dfa70) 

- Server synchronizing.
  - Status: `Done` | Commit: [`c5de683`](https://github.com/ArchLeaders/TaskTracker/commit/c5de6836d85d84b658bfad44c56666862d60c98a)

### Website (HTML/CSS)

- Define primitive (global) site styling.
  - Status: `Done` | Commit: `Untracked`

- Implement reponsive scaling and styles.
  - Status: `Done` | Commit: `Untracked`

- Add documentations and download anchor tags.
  - Status: `Done` | Commit: `Untracked`

- Deploy to GitHub pages.
  - Status: `Done` | Commit: `Untracked`
