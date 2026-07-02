\# AI-Assisted Development Workflow



This document explains how AI was used during the development of \*\*Hadō #90: Kurohitsugi\*\*, a Battle Talent VR mod for Meta Quest 3 standalone.



The goal was not to replace the development process, but to use AI as a technical assistant for debugging, planning, documentation and decision-making.



\---



\## 1. Role of AI in the Project



AI was used as a development support tool throughout the project.



The main areas where AI helped were:



\* breaking complex problems into smaller testable steps;

\* debugging Lua scripts and Unity prefab behavior;

\* interpreting Git status, branches, commits and tags;

\* planning safe refactors without breaking working logic;

\* reviewing Addressables and packaging problems;

\* preparing README and technical documentation;

\* organizing the release workflow for GitHub and mod.io.



Every meaningful change was manually tested in Unity and/or directly on Meta Quest 3.



\---



\## 2. Debugging Support



The project involved several runtime issues that required iterative debugging.



AI was used to help reason through:



\* missing Addressable references;

\* wrong script names;

\* invisible inventory icons;

\* trigger / release behavior;

\* repeated casts;

\* visual sequence timing;

\* prefab cleanup;

\* generated build files accidentally tracked by Git;

\* mod.io ZIP packaging errors.



The debugging workflow usually followed this pattern:



1\. Run the mod in Unity or on Meta Quest 3.

2\. Observe the actual behavior.

3\. Collect logs, screenshots or Git output.

4\. Analyze the issue with AI support.

5\. Apply one targeted change.

6\. Test again.

7\. Commit only when the result was stable.



\---



\## 3. Git Workflow Assistance



AI was used to keep the Git workflow structured and safe.



This included:



\* creating and switching branches;

\* checking `git status`;

\* reading commit history;

\* deciding when to merge;

\* preserving release branches;

\* creating and using tags;

\* cleaning generated files from version control;

\* preparing the public portfolio version on `main`.



Important examples:



```text

release/modio-clean-build

modio-0.1.6-public

```



The final public version was merged into `main` only after the mod.io build was confirmed working.



\---



\## 4. Refactoring Assistance



The project evolved through many iterations.



AI was used to plan safer refactors by focusing on small, isolated changes instead of rewriting the whole system at once.



This was especially important because the mod involved:



\* Lua scripts;

\* Unity prefabs;

\* Addressables;

\* custom shaders;

\* multiple visual phases;

\* runtime VR interaction;

\* Battle Talent-specific behavior.



The preferred workflow was:



```text

small change -> test -> confirm -> commit

```



This helped avoid breaking previously working parts of the spell.



\---



\## 5. Documentation Assistance



AI was also used to prepare clear technical documentation for the public repository.



This included:



\* the main README;

\* technical breakdown;

\* release explanation;

\* mod.io packaging notes;

\* portfolio-oriented project presentation.



The goal was to make the repository understandable not only as a mod, but also as a software development project.



\---



\## 6. Human Validation



AI suggestions were never treated as automatically correct.



Every important decision was validated through:



\* manual testing;

\* Unity inspector checks;

\* Battle Talent in-game tests;

\* Meta Quest 3 standalone testing;

\* Git diffs;

\* commit history;

\* mod.io upload/download testing.



This was especially important because Battle Talent modding has many runtime-specific behaviors that cannot be fully verified without testing in-game.



\---



\## 7. What This Project Demonstrates



This project demonstrates the ability to use AI effectively as part of a real development workflow.



Key skills demonstrated:



\* asking precise technical questions;

\* validating generated suggestions;

\* debugging iteratively;

\* using Git safely;

\* documenting technical decisions;

\* managing a release workflow;

\* adapting AI guidance to a real engine/toolkit environment;

\* turning an idea into a published playable mod.



\---



\## 8. Limitations



AI was useful for reasoning and planning, but it could not replace:



\* in-game testing;

\* Unity prefab inspection;

\* Battle Talent runtime behavior checks;

\* Quest standalone performance testing;

\* final judgment on implementation choices.



The practical development work still required manual testing, iteration and technical decision-making.



