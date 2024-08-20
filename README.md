# OsuLBCheck

Utilizes the same base as osustreamcompanion (aka osumemoryprovider or processmemorydatafinder) to:

  - Scan osu! memory to find current beatmap ID
  - Utilizes your API client id / secret to send an API request to osu to get the top 100 scores
  - Filters them to find the first score belonging to a country (in this case, Brazil)
  - Display information about that score
  - When playing, it also shows a live score counter and the score you need to beat above it.

This is not a substitute for country rankings given that it only gets the top 100 scores (aka if something is outside the top 100 scores then it'll say no top 1 country score exists although that is obviously not true).
I could check it through other methods but I didn't really feel like overloading it by doing too much and it works fine for most cases like this.

Still need to:
  - Fix empty beatmap ID bug
  - Make changing the target country easier

Overlay done in winforms mostly for use as a sidebar in OBS or similar. Might change it to learn some different frontend desktop UI framework.
