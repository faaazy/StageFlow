export type SchedulePerformance = {
  id: string;
  stageId: string;
  stageName: string;
  artistId: string;
  artistName: string;
  startTime: string;
  endTime: string;
};

export async function getPerformances(): Promise<SchedulePerformance[]> {
  const res = await fetch("http://localhost:5178/schedule/performances");

  if (!res.ok) {
    throw new Error("we couldnt get all the performances for you(");
  }

  const data = await res.json();

  return data;
}
