import { useQuery } from "@tanstack/react-query";
import { getPerformances } from "../api/scheduleApi";

export function SchedulePage() {
  const { data, isError, isPending, error } = useQuery({
    queryKey: ["performances"],
    queryFn: getPerformances,
  });

  return (
    <>
      {isPending && <p>Loading...</p>}

      {isError && <p>{error.message}</p>}

      <p>Performances: {data?.length}</p>
    </>
  );
}
