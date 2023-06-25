@ImagesInOtherFolder
Feature: Images and markdown file are in different folders

Use case
--

- You have a markdown file linking to one or more images
- The markdown file is located in the folder `./`
- All images are located in the folder `./subdirectory`
- You want to move the images to the folder `./target`

Command line
--

```
malimo --file ./WithTwoImages.md --source-dir ./subdirectory --target-dir ./target
```

Result
--

- `malimo` will ensure that the linked images exist in the folder `./source`
- It will print out which images are moved
- It will move the images from `./source` to `./target` 

Scenario: Specify image folder using `--source-dir`
	Given the file is "WithTwoImages.md"
	And the "--source-dir" argument is configured as "subdirectory"
	And the target directory is configured
	When malimo is executed
	Then the file "noun-island-1479438.png" does not exist in the directory "subdirectory" beneath the source directory
  And the file "noun-starship-3799189.png" does not exist in the directory "subdirectory" beneath the source directory
  And the file "noun-island-1479438.png" exists in the target directory
  And the file "noun-starship-3799189.png" exists in the target directory
  And malimo has exited gracefully