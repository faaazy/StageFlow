import { createBrowserRouter, Navigate } from "react-router-dom";
import { AppLayout } from "../layouts/AppLayout";
import { SchedulePage } from "../pages/SchedulePage";
import { LoginPage } from "../pages/LoginPage";
import { RegisterPage } from "../pages/RegisterPage";
import { RequireAuth } from "../auth/RequireAuth";
import { PublicLayout } from "../layouts/PublicLayout";
import { LandingPage } from "../pages/LandingPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <PublicLayout />,
    children: [
      { index: true, element: <LandingPage /> },
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
    ],
  },
  {
    element: <RequireAuth />,
    children: [
      {
        path: "/organizer",
        element: <AppLayout />,
        children: [
          {
            index: true,
            element: <Navigate to="/organizer/schedule" replace />,
          },
          {
            path: "schedule",
            element: <SchedulePage />,
          },
        ],
      },
    ],
  },
]);
