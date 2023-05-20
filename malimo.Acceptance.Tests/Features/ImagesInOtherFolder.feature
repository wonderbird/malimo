@ImagesInOtherFolder
Feature: Images in other folder
	Use the `--source-dir` argument to specify an images folder. This option is used when the images and the markdown
  file are in different folders.   

Scenario: Markdown file with two existing images
	Given the file is "WithTwoImages.md"
	And the "--source-dir" argument is configured as "subdirectory"
	And the target directory is configured
	When malimo is executed
	Then the file "noun-island-1479438.png" does not exist in the directory "subdirectory" beneath the source directory
  And the file "noun-starship-3799189.png" does not exist in the directory "subdirectory" beneath the source directory
  And the file "noun-island-1479438.png" exists in the target directory
  And the file "noun-starship-3799189.png" exists in the target directory
  And malimo has exited gracefully