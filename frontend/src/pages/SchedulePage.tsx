import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  createPerformance,
  getPerformances,
  type CreatePerformanceRequest,
} from "../api/scheduleApi";
import { formatTime } from "../utils/formatTime";
import { useState } from "react";

export function SchedulePage() {
  const [stageId, setStageId] = useState("");
  const [artistId, setArtistId] = useState("");
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");

  const queryClient = useQueryClient();

  const { data, isError, isPending, error } = useQuery({
    queryKey: ["performances"],
    queryFn: getPerformances,
  });

  const mutation = useMutation({
    mutationFn: createPerformance,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["performances"],
      });
    },

    onError: (error) => {
      console.error(error);
    },
  });

  const formCreateHandler = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    const performance: CreatePerformanceRequest = {
      stageId,
      artistId,
      startTime: new Date(startTime).toISOString(),
      endTime: new Date(endTime).toISOString(),
    };

    mutation.mutate(performance);
  };

  return (
    <>
      {isPending && <p>Loading...</p>}

      {isError && <p>{error.message}</p>}

      <div>
        Performances:
        {/* no performances */}
        {data?.length === 0 && <p>There are no performances yet.</p>}
        {/* performances */}
        {data &&
          data.length > 0 &&
          data.map((performance) => (
            <ul key={performance.id}>
              <li>{performance.artistName}</li>
              <li>{performance.stageName}</li>
              <li>{formatTime(performance.startTime)}</li>
              <li>{formatTime(performance.endTime)}</li>
            </ul>
          ))}
      </div>

      {/* form Create */}
      <form onSubmit={formCreateHandler}>
        <label htmlFor="stage-id">Stage id</label>
        <input
          type="text"
          id="stage-id"
          name="stageId"
          value={stageId}
          onChange={(e) => setStageId(e.target.value)}
        />

        <label htmlFor="artist-id">Artist id</label>
        <input
          type="text"
          id="artist-id"
          name="artistId"
          value={artistId}
          onChange={(e) => setArtistId(e.target.value)}
        />

        <label htmlFor="start-time">Start Time</label>
        <input
          type="datetime-local"
          id="start-time"
          name="startTime"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
        />

        <label htmlFor="end-time">End Time</label>
        <input
          type="datetime-local"
          id="end-time"
          name="endTime"
          value={endTime}
          onChange={(e) => setEndTime(e.target.value)}
        />

        <button type="submit">Create</button>
      </form>

      {/* mutation messages */}
      {mutation.isPending && <p>Creating...</p>}
      {mutation.isError && <p>{mutation.error.message}</p>}
      {mutation.isSuccess && <p>Performance created!</p>}
    </>
  );
}
