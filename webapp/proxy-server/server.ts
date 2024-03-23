import express from "express";
import http from "http";
import path from "path";
import dotenv from "dotenv";

dotenv.config({ path: path.join(__dirname, "./frontend/.env") });
const app = express();

app.use(express.static("assets"));
app.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "./frontend/dist/index.html"));
});
app.get("/assets/*", (req, res) => {
  res.sendFile(path.join(__dirname, "./frontend/dist", req.url));
});

type Config = {
  apiUrl: string;
};

const removeTrailingSlash = (url: string) =>
  url.endsWith("/") ? url.slice(0, -1) : url;

app.get("/config", (_req, res) => {
  const apiUrl = removeTrailingSlash(
    process.env.API_URL || process.env.VITE_API_URL || "http://localhost:3001"
  );
  const config: Config = {
    apiUrl,
  };
  res.json(config);
});

const PORT = process.env.PORT || 3000;

// Create an HTTP server that uses the express app
const server = http.createServer(app);

server.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});
