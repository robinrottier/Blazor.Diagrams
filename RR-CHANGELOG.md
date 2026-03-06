
# RR CHANGELOG

Forked from the excellent Blazor.Diagrams at http://guthub.com/Blazor-Diagrams/Blazor.Diagrams

## Version v0.1.2

- Rebased fork to 3.0.4.1 to include MudBlazor 9 scrint conflict fix
- Packaging to allow sep forks and easier sync to the original repo
- Add animations to Links
	- using a child of LinkModel to specifiy the animation parameters
	- uses DashPatten to create the animation effect
	- add example of animated links in the demo app with interactive buttons
- If node resizing (see below) and snap to grid then resizing snaps also
- Merge some changes from other forks:
	- Node resizing
	- Catch more JS Interop exceptions and more generically
- Updated to .NET 10
	- Copilot
	- Removed the subsequent nullable warnings (mostly by allowing null values using ?)
