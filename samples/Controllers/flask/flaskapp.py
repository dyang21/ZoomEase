import requests

refresh_token_url = 'http://localhost:5000/zoom/refreshToken'
get_meetings_url = 'http://localhost:5000/zoom/getMeetings'
post_meetings_url = 'http://localhost:5000/zoom/postMeetings'

refresh_token_response = requests.get(refresh_token_url, params={'refreshToken': refreshToken, 'clientId': clientId, 'clientSecret': clientSecret})
get_meetings_response = requests.get(get_meetings_url, params={'refToken': refToken})
post_meetings_response = requests.post(post_meetings_url, json={'Token': token, 'Top': top, 'Pass': pass, 'Start': start, 'Dur': dur})
