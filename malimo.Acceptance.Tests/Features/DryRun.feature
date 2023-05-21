@DryRun
Feature: Dry run simulates what would happen
  
Use case
--

- Files shall not be moved 
- You have a markdown file linking to one or more images
- The markdown file and all images are located in the folder `./`
- You want to list the files linked by the markdown file

Command line
--

```
malimo --dry-run --file WithTwoImages.md --target-dir ../any-directory-name
```

Result
--

- The `--dry-run` option prevents `malimo` from changing the file system
- `malimo` will ensure that the linked images exist in the folder `./`
- It will print out in detail which image would be moved

Scenario: Prevent moving files by using the `--dry-run` option
	Given the option "--dry-run" is added
	And the file is "WithTwoImages.md"
	And the target directory is configured
	When malimo is executed
	Then the output matches the regex "Would move '.*/noun-island-1479438.png'"
  And the file "noun-island-1479438.png" exists in the source directory
  And the file "noun-starship-3799189.png" exists in the source directory
  And the file "noun-island-1479438.png" does not exist in the target directory
  And the file "noun-starship-3799189.png" does not exist in the target directory
  And malimo has exited gracefully
