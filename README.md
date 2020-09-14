artist name
- "forbidden words": group, band, trio,quartet, quintet..., (the), 
- dividers: &, ",", /...

artist folder:
- can contain only folders 
- may contain small number of files like: *.jpg, *.txt

album name
- pattern: [9999] xxxx
- look for:
	- similar album names
	- different year similar name
	- different artist similar album name

album folder:
- only files *.flac, *.ape, *.cue, folder.jpg | front.jpg | cover.jpg (1 file), *.txt (0-1 file)
- if contains *.cue file, number of flac | ape should be the same as number of cue files; 
- if number of flac | ape is higher than number of cue files the folder should not contgain cue file
- *.cue should be valid (valid name of flac|cue file)
- bitrate should be 44.1 or 96
- check for duplicate files (different file name)
- album length cca 30  - 80 min
- should not contain other folders

elastic search

artist name db structure:
- name
- normalized name (lower case, the, a, band, group, trio, quartet, quintet... deleted, in basic ascii, array: in case of more artists - dividers:  and, &, coma, dash, feat., featuring)
- number of dirs,
- total volume