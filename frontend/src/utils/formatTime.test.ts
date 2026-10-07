import { describe, expect, test } from "vitest";
import { formatDate, formatTime } from "./formatTime";

describe("format functions", () => {
  test("formats time to the festival timezone", () => {
    expect(formatTime("2026-10-06T10:00:00.000Z")).toBe("13:00");
  });

  test("formats time on the edge of the next day to the festival timezone", () => {
    expect(formatTime("2026-10-06T22:00:00.000Z")).toBe("01:00");
  });

  test("formats date to the festival timezone", () => {
    expect(formatDate("2026-10-06T10:00:00.000Z")).toBe("06.10.2026");
  });

  test("formats date on the edge of the next day to the festival timezone", () => {
    expect(formatDate("2026-10-06T22:30:00.000Z")).toBe("07.10.2026");
  });
});
