\# Technical Breakdown — Hadō #90: Kurohitsugi



This document explains the technical structure of the \*\*Hadō #90: Kurohitsugi\*\* Battle Talent VR mod.



The goal of the project was to create a standalone spell gem for Battle Talent, targeting \*\*Meta Quest 3 standalone\*\*, with a cinematic dark magic sequence inspired by the visual concept of Kurohitsugi.



\---



\## 1. Project Goal



The mod was designed around a charge-and-release spell flow:



1\. The player equips a custom spell gem.

2\. Holding the trigger starts the charge phase.

3\. A palm preview cube appears near the hand.

4\. A ground target indicator shows where the spell will be cast.

5\. Releasing the trigger spawns the Kurohitsugi sequence.

6\. The sequence builds the black coffin effect in stages.

7\. Reiatsu, rain slash, glow and shattering effects complete the cast.



The main challenge was not only creating the visual effect, but making it work as a real Battle Talent mod with correct prefab references, Lua scripts, Addressables, packaging and Meta Quest 3 testing.



\---



\## 2. Main Technologies



\* Unity 2020.3.48f1

\* Battle Talent Mod Toolkit

\* Lua scripting

\* Unity Prefabs

\* Unity Materials and Shaders

\* Unity Addressables

\* Git and GitHub

\* mod.io distribution

\* Meta Quest 3 standalone testing



\---



\## 3. Runtime Interaction Flow



The spell gem uses a VR interaction flow based on trigger input.



\### Charge Phase



When the player holds the trigger:



\* the spell enters a charging state;

\* the palm preview becomes visible;

\* the ground target indicator appears;

\* the target follows the player's hand aiming logic;

\* the spell can be cancelled with Grip.



\### Release Phase



When the trigger is released:



\* the charge visuals are hidden;

\* the target position is confirmed;

\* the Kurohitsugi cast root is spawned;

\* the sequence begins around the selected target area.



\### Grip / Reiatsu Phase



Grip is also used as an additional interaction layer.



Depending on the current state, Grip can:



\* cancel the spell during charge;

\* activate a reiatsu pressure effect outside the release cast flow.



This made the spell feel closer to an actual VR ability instead of a simple projectile.



\---



\## 4. Main Asset Structure



The public source assets are stored under:



```text

Assets/Build/EXP\_Kurohitsugi\_Gem/

```



Important folders:



```text

Animation/

Audio/

Config/

FlyObj/

ICon/

Material/

Models/

Script/

Shader/

VFX/

Weapon/

```



\### Important Runtime Assets



```text

Weapon/EXP\_Kurohitsugi\_Gem.prefab

FlyObj/KurohitsugiCastRoot.prefab

FlyObj/EZ\_KurohitsugiFlyX.prefab

FlyObj/ElementBullet\_Kurohitsugi.prefab

Script/EXP\_Kurohitsugi\_GemScript.txt

Script/EZ\_FlyKurohitsugiScript.txt

Script/EXP\_KurohitsugiRainSlashScript.txt

Script/KurohitsugiDamageBullet.txt

```



\---



\## 5. Sequence Design



The Kurohitsugi effect is divided into multiple staged visual phases.



\### Main phases



1\. Charge preview

2\. Targeting indicator

3\. Line formation

4\. Wall formation

5\. Top closure

6\. Reiatsu buildup

7\. Rain slash impact

8\. Glow phase

9\. Final shattering



This structure made the sequence easier to tune because each visual phase could be adjusted independently.



\---



\## 6. Lua Scripting



Lua scripts are used to drive the runtime behavior of the spell.



The scripting work includes:



\* managing charge state;

\* showing and hiding preview objects;

\* updating target placement;

\* spawning the final cast object;

\* sequencing visual effects;

\* handling rain slash projectiles;

\* triggering damage logic;

\* avoiding repeated casts or stuck state issues.



The main spell script is:



```text

Assets/Build/EXP\_Kurohitsugi\_Gem/Script/EXP\_Kurohitsugi\_GemScript.txt

```



Supporting scripts handle the fly object, explosion/cast sequence, rain slash behavior and damage bullet behavior.



\---



\## 7. Addressables



A major part of the project was making sure Battle Talent could correctly find every runtime asset.



The final Addressable configuration was cleaned so that only the required build folders are included:



```text

Assets/Build/Common

Assets/Build/EXP\_Kurohitsugi\_Gem

```



The prefix was cleared to avoid unwanted address renaming.



Important address fixes included:



```text

LuaScript/KurohitsugiDamageBullet

ICon/EXP\_Kurohitsugi\_Gem

```



These fixes solved runtime loading issues related to missing scripts and invisible inventory icons.



\---



\## 8. mod.io Packaging



The mod was published publicly on mod.io.



The working mod.io ZIP structure required the base mod folder at the root:



```text

EXP\_Hado90\_Kurohitsugi/

├── Android/

├── Android.meta

├── StandaloneWindows/

└── StandaloneWindows.meta

```



A previous packaging approach caused Battle Talent download/finalization errors. The issue was solved by rebuilding the ZIP with the correct folder structure and archive format.



The final working package was published as:



```text

Version: 0.1.6

Tag: modio-0.1.6-public

```



\---



\## 9. Git Workflow



The project used multiple feature, cleanup and release branches.



Important branches:



```text

main

release/modio-clean-build

cleanup/kurohitsugi-unused-scripts

feature/kurohitsugi-script-refactor

feature/kurohitsugi-wall-dissolve

feature/tune-kurohitsugi-timing

```



Important tag:



```text

modio-0.1.6-public

```



The release branch was merged back into `main` after the mod.io version became public and functional.



\---



\## 10. Problems Solved



\### Missing Addressable Script



Problem:



```text

KurohitsugiDamageBullet was not found

```



Cause:



The file name produced an unexpected Addressable name.



Fix:



The script was renamed so the final address became:



```text

LuaScript/KurohitsugiDamageBullet

```



\---



\### Invisible Inventory Icon



Problem:



The gem appeared in-game, but its inventory icon was invisible.



Cause:



Battle Talent expected the icon address to match the item name.



Fix:



The icon was renamed to:



```text

EXP\_Kurohitsugi\_Gem.png

```



Final address:



```text

ICon/EXP\_Kurohitsugi\_Gem

```



\---



\### mod.io Download Finalization Error



Problem:



The mod downloaded from mod.io but failed during finalization.



Cause:



The ZIP package structure did not match the format expected by Battle Talent mod.io packages.



Fix:



The ZIP was rebuilt with the correct root folder:



```text

EXP\_Hado90\_Kurohitsugi/

```



\---



\### Generated Build Output in Git



Problem:



Unity/Toolkit generated output under `Assets/Mods/` was accidentally tracked.



Fix:



Generated output was removed from version control and added to `.gitignore`.



\---



\## 11. AI-Assisted Development



AI was used as a technical support tool during development.



The AI-assisted workflow helped with:



\* breaking problems into testable steps;

\* debugging Unity/Lua behavior;

\* reviewing Git status and branch decisions;

\* planning refactors;

\* preparing documentation;

\* analyzing packaging problems;

\* keeping a structured development log.



All changes were manually tested in Unity and on Meta Quest 3 before being considered stable.



\---



\## 12. Current Status



```text

Version: 0.1.6

Status: Public on mod.io

Main branch: Portfolio-ready

Target device: Meta Quest 3 standalone

```



Future improvements may include:



\* deeper runtime error cleanup;

\* improved damage balancing;

\* more optimized standalone VFX;

\* better in-repo build instructions;

\* additional gesture-based casting experiments.



