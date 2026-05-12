\# Kurohitsugi Battle Talent VR Mod



Unity VR spell prototype for \*\*Battle Talent\*\* on \*\*Meta Quest 3 standalone\*\*, inspired by a dark ritual cube spell sequence.



This project is a personal learning and prototyping repository focused on VR modding, Unity prefab workflows, scripting, spell activation logic, visual effect sequencing, Quest standalone testing, and Git-based version control.



\---



\## Project Status



Current milestone:



```text

v0.1 - Gesture, spawn and Kurohitsugi sequence working

```



At this stage, the project includes:



\- VR spell gem activation flow

\- Trigger-based gesture/cast logic

\- Kurohitsugi object spawn

\- Timed visual sequence

\- Ground line appearance

\- Cube wall/top sequence

\- Spike sequence

\- Quest 3 standalone testing workflow

\- Git version control setup

\- GitHub repository setup



\---



\## Goal



The goal of this project is to recreate a dramatic dark cube spell sequence inside \*\*Battle Talent VR\*\*.



The intended spell flow is:



1\. The player activates the spell through a VR input/gesture.

2\. A target/cast position is selected.

3\. The Kurohitsugi sequence starts at the chosen position.

4\. Ground lines appear first.

5\. The cube walls appear.

6\. The top section appears.

7\. Spikes pierce the structure.

8\. The sequence ends with destruction, damage logic, or visual cleanup.



\---



\## Tech Stack



\- Unity

\- Battle Talent Mod Toolkit

\- Lua scripting

\- C# / Unity components

\- Android build workflow

\- Meta Quest 3 standalone testing

\- Git

\- GitHub

\- AI-assisted development with ChatGPT



\---



\## Repository Structure



```text

Assets/

Packages/

ProjectSettings/

README.md

.gitignore

PROJECT\_PROMPT.md

```



Main Unity project folders:



| Folder | Purpose |

|---|---|

| `Assets/` | Project assets, prefabs, scripts, materials and mod files |

| `Packages/` | Unity package configuration |

| `ProjectSettings/` | Unity project settings |

| `.gitignore` | Files and folders ignored by Git |

| `README.md` | Project documentation |



Generated Unity folders such as `Library/`, `Temp/`, `Logs/` and `UserSettings/` are intentionally excluded from Git.



\---



\## Current Features



\### Working



\- Spell activation gesture

\- Kurohitsugi spawn

\- Main visual sequence

\- Timed activation of sequence elements

\- Local Git repository

\- GitHub remote repository

\- Quest-oriented mod workflow



\### In Progress



\- Area damage logic

\- Final spell cleanup

\- Visual polish

\- TYBW-inspired timing refinement

\- Spike impact tuning

\- Audio and haptic feedback

\- Performance optimization for Meta Quest 3



\---



\## Development Workflow



This project uses Git for version control.



Basic workflow:



```bash

git status

git add .

git commit -m "Describe the change"

git push

```



Before starting a risky change, the project should be in a clean state:



```bash

git status

```



Expected result:



```text

nothing to commit, working tree clean

```



For larger features, a separate branch should be used:



```bash

git checkout -b feature/area-damage

```



Example feature branches:



```text

feature/area-damage

feature/spike-polish

feature/tybw-timing

feature/sound-effects

feature/quest-performance

```



\---



\## Build and Testing Notes



The project is tested on \*\*Meta Quest 3 standalone\*\* using the Battle Talent modding workflow.



Typical development cycle:



1\. Modify Unity scene, prefab or script.

2\. Build the mod for Android/Quest.

3\. Copy the generated mod files to the Quest.

4\. Launch Battle Talent.

5\. Test spell behavior in-game.

6\. Fix issues.

7\. Commit stable changes with Git.

8\. Push the updated version to GitHub.



\---



\## AI-Assisted Development



This project was developed with the support of \*\*ChatGPT\*\* as an AI coding and learning assistant.



ChatGPT was used for:



\- Code generation support

\- Debugging guidance

\- Unity workflow planning

\- Git setup guidance

\- Technical documentation

\- Step-by-step problem solving

\- Iterative improvement of scripts and project structure



The project was tested, integrated and refined manually inside Unity and Battle Talent on Meta Quest 3.



\---



\## Learning Goals



This project is part of a broader learning path toward software development, web development and applied AI-assisted programming.



Main skills practiced:



\- Version control with Git

\- GitHub repository management

\- Unity project organization

\- Debugging complex behavior

\- Iterative development

\- Technical documentation

\- Script-based logic

\- VR input handling

\- Testing on real hardware

\- Using AI tools effectively during development



\---



\## Milestones



\### v0.1



\- Initial working Kurohitsugi sequence

\- Gesture works

\- Spawn works

\- Main sequence works

\- Git repository initialized

\- Project pushed to GitHub



\### Next Milestones



\- Add reliable area damage

\- Add final cleanup logic

\- Improve visual timing

\- Improve spike behavior

\- Add sound effects

\- Add haptic feedback

\- Optimize for Quest 3 performance



\---



\## Notes



This is a personal technical prototype and learning project.



The repository is currently private while the project is under active development.

