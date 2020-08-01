# HS1Randomizer


Randomly generates a new character in the female maker.

Requirements

    A fully updated game (v1.20)
    IPA https://github.com/Eusth/IPA


How To Install

    Download a release (https://github.com/AldebaraanMKII/HS1Randomizer/releases)
    Drag and drop the files into HoneySelect\Plugins


How To use

    Numpad 1 = Randomize everything
    Numpad 2 = Randomize face
    Numpad 3 = Randomize body
    Numpad 4 = Randomize face/body
    Numpad 5 = Random clothes/Accessories



JSON FILE

    This can be edited to change the values used in generation even if the game is running (no need to restart).
    Use https://jsonlint.com/ to see if your json has correct syntax.


KEYCODES

    Keys used to activate the plugin are found here.
    See https://docs.unity3d.com/Manual/class-InputManager.html for a list of keys.


FACESETTINGS

    headId, texFaceId, texFaceDetailId, faceDetailWeight = aren't used for now.
    hair_color_predefined_preset_chance = chance that the plugin will use a preset inside HairColors instead of generating a random hair color.
    hair_color_brightness = brightness for the hair base color.
    hair_color_shine_brightness = brightness for the hair shine color.

    eye_color_match_hair_color_chance = chance that the eye color will be the same as the hair color.
    heterochromia_chance = https://en.wikipedia.org/wiki/Heterochromia_iridum


BODYSETTINGS

    body_detail_chance = chance for body detail like muscle.
    bodyDetailWeight = min/max weight for the above option.


FACEPRESETS and BODYPRESETS
     
     New presets can be added to these sections.


FEMALE_NAMES and FAMILY_NAMES
     
     Names used when generating a random name for a character.




BUGS
     
     Hairs sets from modded characters sometimes f*ck up the end result.


TO-DO

     save preset from card
     save as card hotkey
     log file




