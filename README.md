
![archiwa kurahenu](dist/logo.png)
# Mitsuba Archivizer
Ultymatywne narzędzie do archiwizowania karakao.ork. Pozwala na zapisywanie wszystkich załączonych, generowanie JSON'a do dalszej obróbki, a także w pełni samodzielnego, pozbawionego JavaScripta pliku HTML wraz z **działającymi wordfilterami,** spoilerami, **kolorowymi nazwami**, licznikiem postów i wszystkim co można sobie wymarzyć.
> \>kucowanie scrapera w c#

# Usage

```
MitsubaArchivizer
wykucowane by DJMATI

  html     Parses thread data and generates a human-readable, self-contained HTML page.

	USAGE:
	Generate HTML, download media from a mixed list of URLs/thread-ids into a custom directory:
	  dotnet MitsubaArchivizer.dll html --out /home/anon/archiwa-kurahenu/ b_2137 c_1337 https://karakao.ork/p/res/1488.html
	Generate HTML, download media, download all thumbnails, restrict to certain extensions:
	  dotnet MitsubaArchivizer.dll html --extensions gif,png,jpg --thumbnails <arg1> <arg2> ... <argN>
	Generate HTML with custom style:
	  dotnet MitsubaArchivizer.dll html --style space.css <arg1> <arg2> ... <argN>
	Generate HTML and prettify it:
	  dotnet MitsubaArchivizer.dll html --prettify <arg1> <arg2> ... <argN>
	Generate HTML without any media (text-only):
	  dotnet MitsubaArchivizer.dll html --no-media <arg1> <arg2> ... <argN>

	  --no-media               Don't download media files.
	  --prettify               Prettify output HTML file.
	  --no-names               Use randomly-chosen names instead of plain text.
	  --no-colored-names       Use randomly-chosen color for names.
	  --no-samefag-count       Include post-count next to the poster ID.
	  --style                  (Default: dark_roach.css) Go check out 'Resources/styles' directory.
	  -e, --extensions         Comma separated list of allowed media files extensions.
	  -o, --out                Output directory, defaults to current working directory.
	  --dont-separate-media    Don't group media files by their extensions.
	  -t, --thumbnails         Download media thumbnails. Animated previews for videos.
	  --help                   Display this help screen.
	  value pos. 0             List of either: URLs pointing to a thread or thread-identificators.

  json     Parses thread data and serializes it into a single json file.

	USAGE:
	Serialize into JSON, download media from a mixed list of URLs/thread-ids into a custom directory:
	  dotnet MitsubaArchivizer.dll json --out /home/anon/archiwa-kurahenu/ b_2137 c_1337 https://karakao.ork/p/res/1488.html
	Serialize into JSON, download media, download all thumbnails, restrict to certain extensions:
	  dotnet MitsubaArchivizer.dll json --extensions gif,png,jpg --thumbnails <arg1> <arg2> ... <argN>
	Serialize into human-readable JSON:
	  dotnet MitsubaArchivizer.dll json --formatted <arg1> <arg2> ... <argN>
	Serialize into JSON without any media (text-only):
	  dotnet MitsubaArchivizer.dll json --no-media <arg1> <arg2> ... <argN>

	  --no-media               (Default: false) Don't download media files.
	  -f, --formatted          Serialize into human-readable JSON.
	  -e, --extensions         Comma separated list of allowed media files extensions.
	  -o, --out                Output directory, defaults to current working directory.
	  --dont-separate-media    Don't group media files by their extensions.
	  -t, --thumbnails         Download media thumbnails. Animated previews for videos.
	  --help                   Display this help screen.
	  value pos. 0             List of either: URLs pointing to a thread or thread-identificators.

  media    Downloads all pics/videos from a thread.

	  USAGE:
	Download media from a mixed list of URLs/thread-ids into a custom directory:
	  dotnet MitsubaArchivizer.dll media --out /home/anon/archiwa-kurahenu/ b_2137 c_1337 https://karakao.ork/p/res
	/1488.html
	Download media, download all thumbnails, restrict to certain extensions:
	  dotnet MitsubaArchivizer.dll media --extensions gif,png,jpg --thumbnails <arg1> <arg2> ... <argN>

	  -e, --extensions         Comma separated list of allowed media files extensions.
	  -o, --out                Output directory, defaults to current working directory.
	  --dont-separate-media    Don't group media files by their extensions.
	  -t, --thumbnails         Download media thumbnails. Animated previews for videos.
	  --help                   Display this help screen.
	  value pos. 0             List of either: URLs pointing to a thread or thread-identificators.

  help     Display more information on a specific command.
```

# License (MIT)

    Copyright (c) 2019 kvdrrrrr

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
