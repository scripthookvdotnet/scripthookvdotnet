# Contributing to ScriptHookVDotNet

Thank you for considering contributing to ScriptHookVDotNet (SHVDN)! We appreciate your time and effort to make this project better.

The following guidelines will help you get started with the contribution process. If you have any doubts or want to discuss the development of SHVDN, you can start discussions on our [GitHub Discussions][discussions_github].

## Table of Contents

- [Contributing to ScriptHookVDotNet](#contributing-to-scripthookvdotnet)
  - [Table of Contents](#table-of-contents)
  - [Code of Conduct](#code-of-conduct)
  - [How Can You Contribute?](#how-can-you-contribute)
    - [Reporting Issues](#reporting-issues)
    - [Suggesting Enhancements](#suggesting-enhancements)
    - [Documentation](#documentation)
    - [Pull Requests](#pull-requests)
  - [Getting Started](#getting-started)
    - [Testing](#testing)
    - [Debugging](#debugging)
  - [Style Guidelines](#style-guidelines)
  - [Commit Guidelines](#commit-guidelines)
  - [License](#license)

## Code of Conduct

Please review and adhere to our [Code of Conduct](CODE_OF_CONDUCT.md) to foster a respectful and inclusive community.

## How Can You Contribute?

### Reporting Issues

If you find a bug, please [create an issue][issue_tracker] in our issue tracker. Include as much detail as possible to help us understand and reproduce the problem. You may want to include snippets of source code, screenshots, etc.

**Note**: The issue tracker should be only for bug reports, feature requests, and enhancement suggestions. If you are having an issue and you are not sure if it is a bug or not, ask on our [GitHub Discussions][discussions_github] first.

### Suggesting Enhancements

We welcome suggestions for new features, improvements, or changes to the project. You can submit your ideas by [creating an issue][issue_tracker] using the "Feature request" template (or manually create one with the "enhancement" label).

### Documentation

We use the GitHub Wiki on this repository *for now*, and it may be replaced by an alternative such as GitBook. If you wish to contribute to our Wiki, you can submit your ideas by [creating an issue][issue_tracker].

### Pull Requests

We encourage you to submit pull requests to contribute directly to the project. Here's how you can do it:

1. Fork the repository and create your branch from `main`.
2. Make your changes, following the [style guidelines](#style-guidelines).
3. Ensure your code passes any existing tests.
4. Write clear and concise commit messages following our [commit guidelines](#commit-guidelines).
5. Submit a pull request, referencing any related issues.

We'll review your pull request, provide feedback, and work with you to get it merged.

## Getting Started

1. If you haven't installed Visual Studio 2022+, install one. You can use Visual Studio 2019 for now, but the support may be dropped in the future.
2. Clone this repository:
```ps
   > git clone https://github.com/scripthookvdotnet/scripthookvdotnet.git
   > cd scripthookvdotnet
```
3. Build the solution. The output directory for the Debug build and the Release build are `[root directory]/bin/Debug` and `[root directory]/bin/Release` respectively. You will see `ScriptHookVDotNet.asi` (core runtime module), `ScriptHookVDotNet2.dll` (v2 API module), and `ScriptHookVDotNet3.dll` (v3 API module) as binary files in one of the output directories. You will also see the debug symbol files `ScriptHookVDotNet.pdb`, `ScriptHookVDotNet2.pdb`, and `ScriptHookVDotNet3.pdb`, which may help you diagnose issues of said binary files more easily.
4. Put the `.asi` and `.dll` files mentioned above, and optionally `.pdb` files, to the root directory of the game.

### Testing

SHVDN uses `xUnit` for its test suite. The tests for the v3 API are located in the `scripting_v3_tests/` directory.

Tests are not required in pull requests due to the complexity of making good test cases with the game, but they are greatly appreciated.

### Debugging

You would have to debug SHVDN by running SHVDN and seeing how it works. Since the game has an anti-debugging feature that crashes the game process and that works **even in Story Mode**, you will not be able to keep debugging with a debugger. b2802 made its anti-debugging system more aggressive, but still lets you debug with a debugger for a considerable amount of time. However, some game updates after b2802 made the game even more aggressive, so you can't really debug with a debugger without debug flag hooks because [the game always and almost immediately detects a debugger](https://discord.com/channels/318621297057988608/318626093013925889/1354726801355833374).

## Style Guidelines

SHVDN uses our [.editorconfig](.editorconfig) with a maximum line length of 120 characters.

Older parts of the codebase might not strictly adhere to our .editorconfig. When contributing to the project, do not reformat code unrelated to your changes.

## Commit Guidelines

We try to follow [Conventional Commits][conventional_commits] with our custom change types and scopes to make it easier to generate changelogs. Write meaningful commit messages to make the project history and release notes more understandable. If the change is significant or complex, please include a commit description providing more details.

Here's our custom rules Conventional Commits explicitly mention:
- Descriptions of subject lines, which are substrings after colons in first lines of commit messages, should be written in the imperative mood when they start with verbs.
- Descriptions of subject lines should start with lowercase letters, not uppercase ones.
  - This rule does not apply whe they mention proper nouns as first words, but for words for code such as names of method, properties and fields, it is strongly recommended to wrap them with pairs of backticks.
  - For example, `feat(core): add support for late script binding` and ``fix(api-v3): `Entity.Model` returning incorrect values`` are considered valid, but `docs(api-v3): Correct grammar errors` is considered invalid.

### Commit Types

- `feat`
  - creates a new feature or changes something visible to users when using SHVDN as binaries, or changes some of the API
  - scopes are required
- `fix`
  - fixes a previously discovered failure/bug
  - scopes are required
- `build`
  - changes to local repository build system and tooling
  - scopes are optional
- `ci`
  - changes to CI configuration and CI-specific tooling
  - scopes are forbidden
- `docs`
  - changes that exclusively affect documentation
  - scopes are optional
- `refactor`
  - refactors without any change in functionality or API (includes style changes)
  - scopes are optional
- `perf`
  - improves performance without any change in functionality or API
  - scopes are required
- `test`
  - improvements or corrections made to the project's test suite
  - scopes are optional

### Commit Scopes

The following scopes can be used with parentheses:

- `core`: for the core ASI module `ScriptHookVDotNet.asi`
- `api-v2`: for the v2 scripting API module `ScriptHookVDotNet2.dll`
- `api-v3`: for the v3 scripting API module `ScriptHookVDotNet3.dll`
- `examples`: for script examples

### Examples

```
fix(api-v3): `Entity.Model` returning incorrect values
```
```
docs(api-v2,api-v3): correct grammar errors across scripting docs
```

## License

By contributing to this project, you agree that your contributions will be licensed under the [project's license](LICENSE.txt).

[issue_tracker]: https://github.com/scripthookvdotnet/scripthookvdotnet/issues
[discussions_github]: https://github.com/scripthookvdotnet/scripthookvdotnet/discussions
[conventional_commits]: https://www.conventionalcommits.org/en/v1.0.0/
