# Windows Context Menu Converter (PDF)

A Windows Explorer context menu extension that can:
- Convert supported files to PDF
- Concatenate multiple supported files into a single PDF (`merged.pdf`)

## Supported input types
- Text: `.txt`
- Images: `.png`, `.jpg`, `.jpeg`

## How it works (high level)
- Uses SharpShell to integrate into the Windows Explorer context menu.
- Uses iText to generate PDFs.
- The “Concat” feature converts each selected file into a temporary PDF in memory,
  then merges all parts into one output document.

## Build / Run
This project targets **.NET Framework 4.8** and builds as a **Class Library**
(shell extension).

Typical steps:
1. Restore NuGet packages
2. Build the solution
3. Register the shell extension (developer workflow)

> Note: Shell extensions run inside Explorer. Use with care, and test on a dev
> machine / VM.

## Project status
Work in progress (WIP). This project is not finished yet and may contain bugs,
edge cases, or unfinished features. Use at your own risk.

## License
See `LICENSE`.

## Third-party libraries
See `THIRD_PARTY_NOTICES.md`.
