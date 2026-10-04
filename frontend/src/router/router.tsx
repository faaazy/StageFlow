import { createBrowserRouter } from "react-router-dom";
import { AppLayout } from "../layouts/AppLayout";
import { SchedulePage } from "../pages/SchedulePage";

export const router = createBrowserRouter([
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
