import { cleanup, screen } from "@testing-library/react";
import { afterEach, describe, expect, test, vi } from "vitest";
import { getPerformances } from "../api/scheduleApi";
import { SchedulePage } from "./SchedulePage";
import { renderWithQueryClient } from "../test/test-utils";

vi.mock("../api/scheduleApi", () => ({
  getPerformances: vi.fn(),
  createPerformance: vi.fn(),
}));

afterEach(() => cleanup());

describe("SchedulePage", () => {
  test("shows an empty state when there are no performances", async () => {
    vi.mocked(getPerformances).mockResolvedValue([]);

    renderWithQueryClient(<SchedulePage />);

    expect(
      await screen.findByText("There are no performances yet."),
    ).toBeInTheDocument();
  });

  test("shows an error when loading performances fails", async () => {
    vi.mocked(getPerformances).mockRejectedValue(new Error("API unavailable"));

    renderWithQueryClient(<SchedulePage />);

    expect(await screen.findByText("API unavailable")).toBeInTheDocument();
  });

  test("renders performance details when API returns data", async () => {
    vi.mocked(getPerformances).mockResolvedValue([
      {
        id: "performance-1",
        stageId: "stage-1",
        stageName: "Main Stage",
        artistId: "artist-1",
        artistName: "Aurora",
        startTime: "2026-10-06T10:00:00.000Z",
        endTime: "2026-10-06T12:00:00.000Z",
      },
    ]);

    renderWithQueryClient(<SchedulePage />);

    expect(await screen.findByText("Aurora")).toBeInTheDocument();
    expect(screen.getByText("Main Stage")).toBeInTheDocument();
    expect(screen.getByText("13:00")).toBeInTheDocument();
    expect(screen.getByText("15:00")).toBeInTheDocument();
    expect(screen.getByText("06.10.2026")).toBeInTheDocument();
  });
});
