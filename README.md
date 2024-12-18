# Windows Context Menu Utility

This utility adds four copy commands to the Windows context menu. It compiles into an executable that can be referenced in the Windows registry.

It includes four copy functions to be used when right-clicking on files or folders:
* Copy Content - copies the content of a file (intended for text based files)
* Copy File Name - copies the full name of a file, including the extension
* Copy Full Path - copies the full path to a folder or file without quotation marks
* Copy Unix Path - copies the full path of a folder or file in Unix path format 

Each command will copy information onto the Windows clipboard.

## Background

The inspiration for each of these commands is to make my day-to-day work as an engineer more efficient. 

**Copy Content** is used to grab all the content of a text-based file without having to open it in an application such as notepad.exe. This makes moving data from a file into another application more efficient. A common use case for me is grabbing the content of a JSON file and pasting it into Postman to troubleshoot a problem. It will copy the content of any file, even a binary file, but the code can be modified after downloading it to restrict this action to text files. [Here is an explanation of why that feature was added and how it was implemented.](https://medium.com/@mckrex/65c9611038f1)

**Copy File Name** is used to get the complete name of a file, including the extension. I made it to replace the previous workflow I used to get the whole file name: right-click on a file and select "Rename", hit Ctrl-A to select the whole name including the extension, then hit Ctrl-C to copy. This command reduces that to one step.

**Copy Full Path** is redundant because Windows has a built-in context menu copy command that does the same thing: Copy Path. But Copy Path adds quotes around the file name, which I usually want to remove. Copy Full Path copies the path without extra quotes.

**Copy Unix Path** copies the path in a unix format: /c/myfolder/myfile.txt. I use this because it's convenient for pasting file paths into Bash for Windows.

## Technologies
C#  
.NET 8.0  
TextCopy - https://github.com/CopyText/TextCopy  
MSTest

## Usage
Publish the project to the desired folder and reference the executable in the Windows registry. The file *shell-utility_add-shell-commands.reg* is provided as a template to use to import the appropriate values. Replace "<app_publish_location>" in the .reg file with the publish path of the executable.

### File Context Menu

The context menu for files is located at `HKEY_CLASSES_ROOT\*\shell`. Each command requires a key with a friendly name for the action and a sub-key called `command`. The value of command is the full path to the executable followed by arguments. In the arguments, `%1` represents the selected item. Below is the .reg import for Copy Content:
```
[HKEY_CLASSES_ROOT\*\shell\KX Copy Content]
[HKEY_CLASSES_ROOT\*\shell\KX Copy Content\command]
@="C:\\<app_publish_location>\\ShellUtility.exe \"%1\" content"
``````

Below are the commands for all actions that can be applied to files:

```
Copy Content     ShellUtility.exe "%1" content  
Copy File Name   ShellUtility.exe "%1" name 
Copy Full Path   ShellUtility.exe "%1" path 
Copy Unix Path   ShellUtility.exe "%1" unix 
```
### Folder Context Menu
The context menu for files is located at `HKEY_CLASSES_ROOT\Directory\shell`. The same registry keys and command syntax applies to directories, but only two commands are usable: Copy Full Path and Copy Unix Path.

```
Copy Full Path   ShellUtility.exe "%1" path 
Copy Unix Path   ShellUtility.exe "%1" unix 
```

## Testing
Tests are included in the ShellUtilityTests project. Most of these are dedicated to the problem of correctly identifying a text-based, human-readable file from a binary file. [This problem is discussed in this post.](https://medium.com/@mckrex/65c9611038f1)