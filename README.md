# TicketsAvailabilityAlerting
The app can periodically search a website for the specified keywords.
If any of them are found, the app makes a beep sound to notify the user and additionally it sends them an e-mail.

This app can be used for instantly notifying the user if tickets are available to the public, so they can reserve good seats.

USAGE:   
TicketsAvailabilityAlerting --testingmode --soundcheck
	
or
	
TicketsAvailabilityAlerting --testingmode --mailcheck \<user-email\>

or 

TicketsAvailabilityAlerting --normalmode 
                            --url \<URL\>
			    --keywords \<comma-separated-search-keywords\> 
                            --timer \<timer-in-seconds\>
			    --email \<user-email\>

OPTIONS:

--testingmode:               Test the app for making beep sound or sending email (this option always goes first).

--soundcheck:                Makes a beep sound.

--mailcheck \<user-email\>:    Sends a testing e-mail.

--normalmode:               Normal mode of the app (this option always goes first).

--url \<URL\>:               The url of the website to search.

--keywords \<keywords\>:       The keywords which the website is searched with (comma-separated).

--timer \<timer-in-seconds\>:  Set every how many seconds the website is searched.

--email \<user-mail\>:         The e-mail of the user to be notified if tickets are available.
