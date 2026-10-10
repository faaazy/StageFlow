import { cleanup, render, screen } from "@testing-library/react";
import { afterEach, describe, expect, test } from "vitest";
import { createMemoryRouter, RouterProvider } from "react-router-dom";
import { AppLayout } from "./AppLayout";

afterEach(() => cleanup());

describe("AppLayout", () => {
  test("renders the brand and nested page", () => {
    const router = createMemoryRouter(
      [
        {
          path: "/",
          element: <AppLayout />,
          children: [
            {
              path: "schedule",
              element: <h1>Schedule test content</h1>,
            },
          ],
        },
      ],
      { initialEntries: ["/schedule"] },
    );

    render(<RouterProvider router={router} />);

    expect(
      screen.getByText("StageFlow · Festival Operations Platform"),
    ).toBeInTheDocument();

    expect(
      screen.getByRole("heading", { name: "Schedule test content" }),
    ).toBeInTheDocument();
  });
});
