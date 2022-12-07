FROM node

WORKDIR /src

ADD package.json /src 

RUN npm i --silent

ADD . .

RUN npm run build 

CMD npm start
