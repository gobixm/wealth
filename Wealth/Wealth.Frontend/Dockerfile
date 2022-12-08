FROM node

WORKDIR /src

ADD package.json /src 

RUN npm i

ADD . .

RUN npm run build 

CMD npm start
