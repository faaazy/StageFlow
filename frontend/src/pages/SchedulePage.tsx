import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  createPerformance,
  getPerformances,
  type CreatePerformanceRequest,
  type SchedulePerformance,
} from "../api/scheduleApi";
import { formatDate, formatTime } from "../utils/formatTime";
import { StatusAlert } from "../components/StatusAlert";
import { useState } from "react";

const conflictMessage = "This performance conflicts with another performance.";

function getDurationLabel(startTime: string, endTime: string): string | null {
  const start = new Date(startTime).getTime();
  const end = new Date(endTime).getTime();

  if (!Number.isFinite(start) || !Number.isFinite(end) || end <= start) {
    return null;
  }

  const totalMinutes = Math.round((end - start) / 60000);
  const hours = Math.floor(totalMinutes / 60);
  const minutes = totalMinutes % 60;

  if (hours === 0) {
    return `${minutes} min`;
  }

  return minutes === 0 ? `${hours} h` : `${hours} h ${minutes} min`;
}

const labelClassName = "mb-[6px] block text-[13px] text-muted";

const inputClassName =
  "block h-10 w-full rounded-[6px] border border-border bg-surface px-3 text-[15px] text-text transition-colors placeholder:text-muted focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none dark:bg-bg";

const listColumnsClassName = "sm:grid-cols-[170px_minmax(0,1fr)_auto]";

function Spinner() {
  return (
    <span
      aria-hidden="true"
      className="size-4 animate-spin rounded-full border-2 border-current border-t-transparent"
    />
  );
}

function RowSkeleton() {
  return (
    <li className="px-5 py-[14px]">
      <div className="animate-pulse">
        <div className={`grid gap-3 sm:items-center ${listColumnsClassName}`}>
          <div className="h-4 w-24 rounded bg-border" />
          <div className="h-4 w-44 rounded bg-border" />
          <div className="h-5 w-20 rounded bg-border sm:justify-self-end" />
        </div>
      </div>
    </li>
  );
}

function groupByDate(performances: SchedulePerformance[]) {
  const groups = new Map<string, SchedulePerformance[]>();

  for (const performance of performances) {
    const date = formatDate(performance.startTime);
    const group = groups.get(date);

    if (group) {
      group.push(performance);
    } else {
      groups.set(date, [performance]);
    }
  }

  return [...groups].map(([date, items]) => ({ date, items }));
}

function PerformanceRow({ performance }: { performance: SchedulePerformance }) {
  const duration = getDurationLabel(performance.startTime, performance.endTime);

  return (
    <li
      className={`grid gap-x-5 gap-y-2 px-5 py-[14px] transition-colors hover:bg-[color-mix(in_srgb,var(--text)_6%,var(--surface))] sm:items-center ${listColumnsClassName}`}
    >
      <div className="order-2 sm:order-1">
        <p className="font-mono text-[18px] text-text tabular-nums">
          <span>{formatTime(performance.startTime)}</span>
          <span> – </span>
          <span>{formatTime(performance.endTime)}</span>
        </p>
        {duration && (
          <p className="mt-0.5 font-mono text-[13px] text-muted tabular-nums">
            {duration}
          </p>
        )}
      </div>

      <div className="order-1 min-w-0 sm:order-2">
        <p className="truncate text-[16px] font-semibold text-text">
          {performance.artistName}
        </p>
        <p className="mt-0.5 truncate font-mono text-xs text-muted">
          {performance.artistId}
        </p>
      </div>

      <div className="order-3 flex min-w-0 items-center gap-2 sm:justify-end">
        <span className="max-w-full truncate rounded-[4px] border border-border px-2 py-[2px] text-[13px] font-medium text-text">
          {performance.stageName}
        </span>
      </div>
    </li>
  );
}

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

  const performanceCount = data?.length ?? 0;
  const stageCount = new Set(data?.map((p) => p.stageId)).size;
  const groups = data ? groupByDate(data) : [];
  const isConflict =
    mutation.isError && mutation.error.message === conflictMessage;

  return (
    <div className="space-y-6">
      <div className="flex flex-wrap items-end justify-between gap-x-6 gap-y-2 border-b border-border pb-4">
        <div className="min-w-0">
          <h1 className="text-[32px] font-semibold tracking-tight text-text">
            Schedule
          </h1>
          <p className="mt-1.5 text-[15px] text-muted">
            Running order for every artist set, in stage time.
          </p>
        </div>
        {data && (
          <p className="shrink-0 font-mono text-[13px] text-muted tabular-nums">
            {performanceCount} {performanceCount === 1 ? "set" : "sets"} ·{" "}
            {stageCount} {stageCount === 1 ? "stage" : "stages"}
          </p>
        )}
      </div>

      <div className="grid gap-6 xl:grid-cols-[minmax(0,1fr)_21rem] xl:items-start">
        <section aria-label="Performances" aria-busy={isPending}>
          {isPending && (
            <div className="overflow-hidden rounded-[6px] border border-border bg-surface">
              <div className="flex items-center gap-2 border-b border-border px-5 py-3 text-[13px] text-muted">
                <Spinner />
                Loading...
              </div>
              <ul className="divide-y divide-border">
                <RowSkeleton />
                <RowSkeleton />
                <RowSkeleton />
                <RowSkeleton />
              </ul>
            </div>
          )}

          {isError && (
            <StatusAlert tone="error" title="Request failed">
              {error.message}
            </StatusAlert>
          )}

          {!isPending && !isError && data?.length === 0 && (
            <div className="rounded-[6px] border border-dashed border-border bg-surface px-6 py-16 text-center">
              <p className="text-[15px] font-medium text-text">
                There are no performances yet.
              </p>
              <p className="mt-1.5 text-[15px] text-muted">
                Book the first set to start building the running order.
              </p>
            </div>
          )}

          {data && data.length > 0 && (
            <div className="overflow-hidden rounded-[6px] border border-border bg-surface">
              {groups.map((group) => (
                <section
                  key={group.date}
                  aria-label={`Performances on ${group.date}`}
                >
                  <div className="flex items-center justify-between gap-3 border-b border-border bg-bg px-5 py-[14px]">
                    <h2 className="font-mono text-[14px] font-bold text-text tabular-nums">
                      {group.date}
                    </h2>
                    <span className="text-[12px] tracking-wider text-muted uppercase tabular-nums">
                      {group.items.length}{" "}
                      {group.items.length === 1 ? "set" : "sets"}
                    </span>
                  </div>

                  <ul className="divide-y divide-border">
                    {group.items.map((performance) => (
                      <PerformanceRow
                        key={performance.id}
                        performance={performance}
                      />
                    ))}
                  </ul>
                </section>
              ))}
            </div>
          )}
        </section>

        <section aria-label="Create performance" className="xl:sticky xl:top-8">
          <div className="rounded-[6px] border border-border bg-surface p-5">
            <h2 className="font-heading text-[18px] font-semibold text-text">
              Create performance
            </h2>
            <p className="mt-1 text-[13px] text-muted">
              Submitted in UTC, shown in Europe/Tallinn.
            </p>

            <form onSubmit={formCreateHandler} className="mt-5 space-y-4">
              <div>
                <label className={labelClassName} htmlFor="stage-id">
                  Stage ID
                </label>
                <input
                  type="text"
                  id="stage-id"
                  name="stageId"
                  value={stageId}
                  onChange={(e) => setStageId(e.target.value)}
                  placeholder="00000000-0000-0000-0000-000000000000"
                  autoComplete="off"
                  spellCheck={false}
                  className={`${inputClassName} font-mono text-[13px]`}
                />
              </div>

              <div>
                <label className={labelClassName} htmlFor="artist-id">
                  Artist ID
                </label>
                <input
                  type="text"
                  id="artist-id"
                  name="artistId"
                  value={artistId}
                  onChange={(e) => setArtistId(e.target.value)}
                  placeholder="00000000-0000-0000-0000-000000000000"
                  autoComplete="off"
                  spellCheck={false}
                  className={`${inputClassName} font-mono text-[13px]`}
                />
              </div>

              <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-1">
                <div>
                  <label className={labelClassName} htmlFor="start-time">
                    Start time
                  </label>
                  <input
                    type="datetime-local"
                    id="start-time"
                    name="startTime"
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                    className={`${inputClassName} font-mono`}
                  />
                </div>

                <div>
                  <label className={labelClassName} htmlFor="end-time">
                    End time
                  </label>
                  <input
                    type="datetime-local"
                    id="end-time"
                    name="endTime"
                    value={endTime}
                    onChange={(e) => setEndTime(e.target.value)}
                    className={`${inputClassName} font-mono`}
                  />
                </div>
              </div>

              <div className="space-y-4 pt-0">
                {mutation.isPending && (
                  <StatusAlert tone="pending">Creating...</StatusAlert>
                )}
                {mutation.isError &&
                  (isConflict ? (
                    <StatusAlert tone="conflict" title="Schedule conflict">
                      {mutation.error.message}
                    </StatusAlert>
                  ) : (
                    <StatusAlert tone="error">
                      {mutation.error.message}
                    </StatusAlert>
                  ))}
                {mutation.isSuccess && (
                  <StatusAlert tone="success">Performance created!</StatusAlert>
                )}

                <button
                  type="submit"
                  disabled={mutation.isPending}
                  className="inline-flex h-11 w-full items-center justify-center gap-2 rounded-[6px] bg-accent px-4 text-[15px] font-semibold text-accent-fg transition-[filter] hover:brightness-95 focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none disabled:cursor-not-allowed disabled:opacity-50 dark:hover:brightness-110"
                >
                  {mutation.isPending && <Spinner />}
                  {mutation.isPending ? "Creating..." : "Create"}
                </button>
              </div>
            </form>
          </div>
        </section>
      </div>
    </div>
  );
}
