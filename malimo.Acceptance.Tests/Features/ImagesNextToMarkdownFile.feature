@ImagesNextToMarkdownFile
Feature: Images next to markdown file
	`malimo` moves images located in the same folder as the markdown file into the target directory. 

Scenario: Markdown file with two existing images
	Given the file is "WithTwoImages.md"
	And the target directory is configured
	When malimo is executed
	Then the file "noun-island-1479438.png" does not exist in the source directory
  And the file "noun-starship-3799189.png" does not exist in the source directory
  And the file "noun-island-1479438.png" exists in the target directory
  And the file "noun-starship-3799189.png" exists in the target directory
  And malimo has exited gracefully