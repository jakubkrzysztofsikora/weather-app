# Build stage for Vue.js app
FROM node:lts-alpine as app-build-stage
WORKDIR /app
COPY frontend/package*.json ./
RUN npm install
COPY ./frontend .
RUN npm run build

# Production stage
FROM node:lts-alpine as production-stage
ARG PORT
ENV PORT=${PORT}
WORKDIR /app

RUN mkdir ./frontend
COPY --from=app-build-stage /app/dist ./frontend/dist

# Copy package.json for dependencies (if any) and potential scripts
COPY package*.json ./
COPY tsconfig.json ./
RUN npm install --only=production
COPY server.ts ./

EXPOSE ${PORT}
CMD ["npm", "run", "serve"]
