import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { Auth0Provider } from "@auth0/auth0-react";
import "./index.css";
import { BrowserRouter } from "react-router-dom";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

root.render(
  <React.StrictMode>
    <Auth0Provider
      domain="dev-x388qatxub3ntaam.us.auth0.com"
      clientId="xeyC3EfHtgAtHu4tJ7Gm0lQhuBUD7lmK"
      authorizationParams={{
        redirect_uri: `${window.location.origin}/dashboard`,
      }}
    >
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Auth0Provider>
  </React.StrictMode>
);
