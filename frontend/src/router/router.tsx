import { createBrowserRouter } from "react-router-dom";
import { AppLayout } from "../layouts/AppLayout";
import { SchedulePage } from "../pages/SchedulePage";
import { LoginPage } from "../pages/LoginPage";

export const router = createBrowserRouter([
  {
    path: "/login",
    element: <LoginPage />,
  },
  {
    path: "/",
    element: <AppLayout />,
    children: [
      {
        path: "schedule",
        element: <SchedulePage />,
      },
    ],
  },
]);
