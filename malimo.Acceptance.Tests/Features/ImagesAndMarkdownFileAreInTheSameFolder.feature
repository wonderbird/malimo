@ImagesNextToMarkdownFile
Feature: Images and markdown file are in the same folder
  
Use case
--

- You have a markdown file linking to one or more images.
- The markdown file and all images are located in the folder `./`
- You want to move the images to the folder `./target`

Command line
--

```
malimo --file ./WithTwoImages.md --target-dir ./target
```

Result
--

- `malimo` will ensure that the linked images exist in the folder `./`
- It will print out which images are moved
- It will move the images from `./` to `./target` 

Scenario: Images and markdown file in same folder
	Given the file is "WithTwoImages.md"
	And the target directory is configured
	When malimo is executed
	Then the file "noun-island-1479438.png" does not exist in the source directory
  And the file "noun-starship-3799189.png" does not exist in the source directory
  And the file "noun-island-1479438.png" exists in the target directory
  And the file "noun-starship-3799189.png" exists in the target directory
  And malimo has exited gracefully