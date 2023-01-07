# Morpeh Tanks
A simple tank game example using [Morpeh ECS framework](https://github.com/scellecs/morpeh),
[Morpeh Helpers](https://github.com/SH42913/morpeh.helpers)
and [Unity Input System](https://github.com/Unity-Technologies/InputSystem). \
It contains usages of Morpeh Providers and examples of ECS Unit tests.

![image](https://user-images.githubusercontent.com/17111024/210394187-b856a391-34b8-4af9-b6bd-5bcaa0d61c99.png)

The game has local multiplayer: one input device - one player, currently only keyboard and gamepads are supported.
Be aware, that was just my test job, so the project has no best practices and **must be used only as an example, not as a guide**.

## REQUIRES ODIN INSPECTOR

If you want to clone the project and dive into it... **ODIN INSPECTOR IS REQUIRED!**

You must be aware that correct work of the project is able only with [Odin Inspector](https://odininspector.com/). I'm
unable to publish it to GitHub. So, make sure you can import Odin Inspector into the project after cloning :)

## Broken [Installer] Main prefab
There's possibility to get broken links in Main Installer after import of Odin.
To solve this just right click prefab and select `Reimport` action. This action must fix links.