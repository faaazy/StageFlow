import { apiFetch } from "./apiClient";

// GET type
export type Festival = {
  id: string;
  organizerId: string;
  status: FestivalStatus;
  name: string;
  description: string | null;
  location: string;
  startDate: string;
  endDate: string;
  timeZone: string;
};

export type FestivalStatus =
  | "Draft"
  | "Published"
  | "OnSale"
  | "Live"
  | "Finished"
  | "Cancelled";

// GET request
export async function getMyFestivals(): Promise<Festival[]> {
  const res = await apiFetch("/festivals/mine");

  if (!res.ok) {
    throw new Error("Failed to load festivals");
  }

  return await res.json();
}

// POST type
export type CreateFestivalRequest = {
  name: string;
  description: string | null;
  location: string;
  startDate: string; // "2027-01-01T10:00:00Z"
  endDate: string;
  timeZone: string; // "Europe/Tallinn"
};

export async function createFestival(
  request: CreateFestivalRequest,
): Promise<Festival> {
  const res = await apiFetch("/festivals", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (res.status === 400) {
    const message: string = await res.json();
    throw new Error(message);
  }

  if (!res.ok) {
    throw new Error("Failed to create festival");
  }

  return res.json();
}
