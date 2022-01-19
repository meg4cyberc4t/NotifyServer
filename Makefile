run:
	docker-compose up --build
	
down: 
	docker-compose down

restart:
	make start
	make down
	
quiet:
	git rm -r --cached .database/** -f
