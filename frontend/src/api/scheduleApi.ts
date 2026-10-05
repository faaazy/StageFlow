// GET type
export type SchedulePerformance = {
  id: string;
  stageId: string;
  stageName: string;
  artistId: string;
  artistName: string;
  startTime: string;
  endTime: string;
};

// GET request
export async function getPerformances(): Promise<SchedulePerformance[]> {
  const res = await fetch("http://localhost:5178/schedule/performances");

  if (!res.ok) {
    throw new Error("we couldnt get all the performances for you(");
  }

  const data = await res.json();

  return data;
}

// POST type
export type CreatePerformanceRequest = {
  stageId: string;
  artistId: string;
  startTime: string;
  endTime: string;
};

export type PerformanceResponse = {
  id: string;
  stageId: string;
  artistId: string;
  startTime: string;
  endTime: string;
};

export async function createPerformance(
  request: CreatePerformanceRequest,
): Promise<PerformanceResponse> {
  const res = await fetch("http://localhost:5178/schedule/performances", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });

  if (res.status === 409) {
    throw new Error("This performance conflicts with another performance.");
  }

  if (!res.ok) {
    throw new Error("Could not create performance.");
  }

  const data = await res.json();

  return data;
}
