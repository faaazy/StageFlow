import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createFestival, getMyFestivals } from "../api/festivalsApi";
import { FestivalStatusLabel } from "../components/FestivalStatusLabel";
import { StatusAlert } from "../components/StatusAlert";
import { formatDate } from "../utils/formatTime";

const columnsClassName =
  "sm:grid-cols-[minmax(0,2fr)_minmax(0,1.4fr)_minmax(0,1fr)_minmax(0,1fr)]";

const skeletonRows = [0, 1, 2];

export function FestivalsPage() {
  const queryClient = useQueryClient();

  const { data, error, isError, isPending } = useQuery({
    queryKey: ["festivals", "mine"],
    queryFn: getMyFestivals,
  });

  const mutation = useMutation({
    mutationFn: createFestival,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["festivals"] }),
  });

  const createFestivalBtnHandler = () => {
    mutation.mutate({
      name: "Test festival",
      description: null,
      location: "Tallinn",
      startDate: "2027-06-15T10:00:00Z",
      endDate: "2027-06-15T13:00:00Z",
      timeZone: "Europe/Tallinn",
    });
  };

  const festivalCount = data?.length ?? 0;

  return (
    <div className="space-y-6">
      <div className="flex flex-wrap items-end justify-between gap-x-6 gap-y-2 border-b border-border pb-4">
        <div className="min-w-0">
          <p className="text-[14px] text-muted">Operations</p>
          <h1 className="mt-1 text-[32px] font-semibold tracking-tight text-text">
            Festivals
          </h1>
        </div>
        {data && festivalCount > 0 && (
          <p className="shrink-0 font-mono text-[13px] text-muted">
            {festivalCount} {festivalCount === 1 ? "festival" : "festivals"}
          </p>
        )}
      </div>

      <section aria-label="Festivals" aria-busy={isPending}>
        {isPending && (
          <div className="overflow-hidden rounded-[6px] border border-border bg-surface">
            <ul className="divide-y divide-border">
              {skeletonRows.map((row) => (
                <li
                  key={row}
                  className={`grid grid-cols-[minmax(0,1fr)_auto] gap-x-4 gap-y-1 px-5 py-4 ${columnsClassName}`}
                >
                  <div className="col-start-1 row-start-1 h-6 w-2/3 animate-pulse rounded bg-border" />
                  <div className="col-start-2 row-start-1 h-6 w-16 animate-pulse rounded bg-border sm:col-start-4" />
                  <div className="col-span-2 row-start-2 h-[21px] w-1/2 animate-pulse rounded bg-border sm:col-span-1 sm:col-start-2 sm:row-start-1" />
                  <div className="col-span-2 row-start-3 h-[23px] w-2/5 animate-pulse rounded bg-border sm:col-span-1 sm:col-start-3 sm:row-start-1" />
                </li>
              ))}
            </ul>
          </div>
        )}

        {isError && (
          <StatusAlert tone="error" title="Error">
            {error.message}
          </StatusAlert>
        )}

        {!isPending && !isError && data?.length === 0 && (
          <div className="rounded-[6px] border border-dashed border-border bg-surface px-6 py-16 text-center">
            <p className="text-[15px] text-muted">You have no festivals yet.</p>
          </div>
        )}

        {!isPending && !isError && data && festivalCount > 0 && (
          <div className="overflow-hidden rounded-[6px] border border-border bg-surface">
            <div
              className={`hidden gap-x-4 border-b border-border bg-bg px-5 py-3 text-[12px] font-medium uppercase tracking-wider text-muted sm:grid ${columnsClassName}`}
            >
              <span>Name</span>
              <span>Dates</span>
              <span>Location</span>
              <span>Status</span>
            </div>

            <ul className="divide-y divide-border">
              {data.map((festival) => {
                const isMuted =
                  festival.status === "Cancelled" ||
                  festival.status === "Finished";
                const textClassName = isMuted ? "text-muted" : "text-text";

                return (
                  <li
                    key={festival.id}
                    className={`grid grid-cols-[minmax(0,1fr)_auto] gap-x-4 gap-y-1 px-5 py-4 transition-colors hover:bg-[color-mix(in_srgb,var(--text)_6%,var(--surface))] sm:items-center ${columnsClassName}`}
                  >
                    <p
                      className={`col-start-1 row-start-1 min-w-0 truncate font-semibold ${textClassName} ${festival.status === "Cancelled" ? "line-through" : ""}`}
                    >
                      {festival.name}
                    </p>
                    <div className="col-start-2 row-start-1 sm:col-start-4">
                      <FestivalStatusLabel status={festival.status} />
                    </div>
                    <p className="col-span-2 row-start-2 font-mono text-[14px] tabular-nums text-muted sm:col-span-1 sm:col-start-2 sm:row-start-1">
                      {formatDate(festival.startDate)} –{" "}
                      {formatDate(festival.endDate)}
                    </p>
                    <p
                      className={`col-span-2 row-start-3 text-[15px] ${textClassName} sm:col-span-1 sm:col-start-3 sm:row-start-1`}
                    >
                      {festival.location}
                    </p>
                  </li>
                );
              })}
            </ul>
          </div>
        )}
      </section>

      <button
        onClick={createFestivalBtnHandler}
        className="mt-5 border border-border px-3"
        disabled={mutation.isPending}
      >
        Create new test festival
      </button>
      {mutation.isError && <p>{mutation.error?.message}</p>}
    </div>
  );
}
