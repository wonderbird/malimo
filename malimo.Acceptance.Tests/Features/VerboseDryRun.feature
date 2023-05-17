@VerboseDryRun
Feature: Verbose Dry Run
  To be sure that malimo does not damage my files
  as a user
  I want malimo to print what would be moved without modifying the files on disk.

Scenario: Add `--dry-run` option
	Given the option "--dry-run" is added
	And the file is "Testfile.md"
	And the target directory is configured
	When malimo is executed
	Then the output matches the regex "Would move '.*/noun-island-1479438.png'"
  And malimo has exited gracefully
