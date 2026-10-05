import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  createPerformance,
  getPerformances,
  type CreatePerformanceRequest,
  type SchedulePerformance,
} from "../api/scheduleApi";
import { formatDate, formatTime } from "../utils/formatTime";
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

const labelClassName =
  "block text-[13px] font-medium text-neutral-600 dark:text-neutral-400";

const inputClassName =
  "block h-10 w-full rounded border border-neutral-300 bg-white px-3 text-[15px] text-neutral-900 transition-colors placeholder:text-neutral-400 focus:border-accent-600 focus:outline-none focus:ring-2 focus:ring-accent-600/20 dark:border-neutral-700 dark:bg-neutral-900 dark:text-neutral-100 dark:placeholder:text-neutral-600 dark:focus:border-accent-400 dark:focus:ring-accent-400/30";

const listColumnsClassName = "sm:grid-cols-[8.5rem_minmax(0,1fr)_10rem]";

function Spinner() {
  return (
    <span
      aria-hidden="true"
      className="size-4 animate-spin rounded-full border-2 border-current border-t-transparent"
    />
  );
}

function StatusAlert({
  tone,
  title,
  children,
}: {
  tone: "error" | "conflict" | "pending" | "success";
  title?: string;
  children: React.ReactNode;
}) {
  const toneClassName = {
    error:
      "border-neutral-300 bg-white text-neutral-700 dark:border-neutral-700 dark:bg-neutral-900 dark:text-neutral-200",
    conflict:
      "border-red-300 border-l-2 border-l-red-600 bg-red-50 text-red-900 dark:border-red-900/70 dark:border-l-red-500 dark:bg-red-950/40 dark:text-red-100",
    pending:
      "border-neutral-300 bg-white text-neutral-600 dark:border-neutral-700 dark:bg-neutral-900 dark:text-neutral-300",
    success:
      "border-emerald-300 bg-emerald-50 text-emerald-900 dark:border-emerald-800 dark:bg-emerald-950/40 dark:text-emerald-100",
  }[tone];

  return (
    <div
      role="status"
      aria-live="polite"
      className={`flex items-start gap-2.5 rounded border px-3 py-2.5 text-[13px] leading-relaxed ${toneClassName}`}
    >
      {tone === "pending" && (
        <span className="mt-0.5">
          <Spinner />
        </span>
      )}
      <div className="min-w-0">
        {title && (
          <p className="font-semibold tracking-wide uppercase">{title}</p>
        )}
        <p className={title ? "mt-0.5 break-words" : "break-words"}>
          {children}
        </p>
      </div>
    </div>
  );
}

function RowSkeleton() {
  return (
    <li className="px-5 py-3.5">
      <div className="animate-pulse">
        <div
          className={`grid gap-3 sm:grid-cols-[8.5rem_minmax(0,1fr)_10rem] sm:items-center`}
        >
          <div className="h-3.5 w-16 rounded bg-neutral-200 dark:bg-neutral-800" />
          <div className="h-3.5 w-44 rounded bg-neutral-200 dark:bg-neutral-800" />
          <div className="h-3.5 w-24 rounded bg-neutral-200 sm:justify-self-end dark:bg-neutral-800" />
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
      className={`grid gap-x-5 gap-y-2 px-5 py-3.5 transition-colors hover:bg-neutral-50 dark:hover:bg-neutral-800/50 sm:items-center ${listColumnsClassName}`}
    >
      <div className="order-2 flex items-baseline gap-2 sm:order-1 sm:block sm:border-l sm:border-neutral-200 sm:pl-3.5 dark:sm:border-neutral-800">
        <span className="text-[15px] font-medium text-neutral-900 tabular-nums dark:text-neutral-50">
          {formatTime(performance.startTime)}
        </span>
        <span className="text-[13px] text-neutral-400 sm:hidden dark:text-neutral-500">
          –
        </span>
        <span className="text-[13px] text-neutral-500 tabular-nums sm:mt-0.5 sm:block dark:text-neutral-400">
          {formatTime(performance.endTime)}
        </span>
        {duration && (
          <span className="text-[13px] text-neutral-400 tabular-nums sm:mt-0.5 sm:block dark:text-neutral-500">
            <span className="sm:hidden">· </span>
            {duration}
          </span>
        )}
      </div>

      <div className="order-1 min-w-0 sm:order-2">
        <p className="truncate text-[15px] font-medium text-neutral-900 dark:text-neutral-50">
          {performance.artistName}
        </p>
        <p className="mt-0.5 truncate font-mono text-xs text-neutral-400 dark:text-neutral-500">
          {performance.artistId}
        </p>
      </div>

      <div className="order-3 flex min-w-0 items-center gap-2 sm:justify-end">
        <span className="max-w-full truncate rounded border border-neutral-200 bg-neutral-50 px-2 py-1 text-[13px] font-medium text-neutral-700 dark:border-neutral-700 dark:bg-neutral-800 dark:text-neutral-200">
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
      <div className="flex flex-wrap items-end justify-between gap-x-6 gap-y-2 border-b border-neutral-200 pb-4 dark:border-neutral-800">
        <div className="min-w-0">
          <h1 className="text-xl font-semibold tracking-tight text-neutral-900 dark:text-neutral-50">
            Schedule
          </h1>
          <p className="mt-1.5 text-[15px] text-neutral-500 dark:text-neutral-400">
            Running order for every artist set, in stage time.
          </p>
        </div>
        {data && (
          <p className="shrink-0 text-[13px] text-neutral-500 tabular-nums dark:text-neutral-400">
            {performanceCount} {performanceCount === 1 ? "set" : "sets"} ·{" "}
            {stageCount} {stageCount === 1 ? "stage" : "stages"}
          </p>
        )}
      </div>

      <div className="grid gap-6 xl:grid-cols-[minmax(0,1fr)_21rem] xl:items-start">
        <section aria-label="Performances" aria-busy={isPending}>
          {isPending && (
            <div className="overflow-hidden rounded-md border border-neutral-200 bg-white dark:border-neutral-800 dark:bg-neutral-900">
              <div className="flex items-center gap-2 border-b border-neutral-200 px-5 py-3 text-[13px] text-neutral-500 dark:border-neutral-800 dark:text-neutral-400">
                <Spinner />
                Loading...
              </div>
              <ul className="divide-y divide-neutral-100 dark:divide-neutral-800">
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
            <div className="rounded-md border border-dashed border-neutral-300 bg-white px-6 py-16 text-center dark:border-neutral-700 dark:bg-neutral-900">
              <p className="text-[15px] font-medium text-neutral-700 dark:text-neutral-100">
                There are no performances yet.
              </p>
              <p className="mt-1.5 text-[15px] text-neutral-500 dark:text-neutral-400">
                Book the first set to start building the running order.
              </p>
            </div>
          )}

          {data && data.length > 0 && (
            <div className="overflow-hidden rounded-md border border-neutral-200 bg-white dark:border-neutral-800 dark:bg-neutral-900">
              <div
                className={`hidden gap-x-5 border-b border-neutral-200 bg-neutral-50 px-5 py-2.5 sm:grid dark:border-neutral-800 dark:bg-neutral-800/40 ${listColumnsClassName}`}
              >
                <span className="text-[11px] font-medium tracking-wider text-neutral-500 uppercase dark:text-neutral-400">
                  Time
                </span>
                <span className="text-xs font-medium tracking-wider text-neutral-500 uppercase dark:text-neutral-400">
                  Artist
                </span>
                <span className="text-right text-xs font-medium tracking-wider text-neutral-500 uppercase dark:text-neutral-400">
                  Stage
                </span>
              </div>

              {groups.map((group, groupIndex) => (
                <section
                  key={group.date}
                  aria-label={`Performances on ${group.date}`}
                >
                  <div
                    className={`flex items-center justify-between gap-3 bg-neutral-50 px-5 py-2 dark:bg-neutral-800/40 ${
                      groupIndex === 0
                        ? "border-b border-neutral-200 dark:border-neutral-800"
                        : "border-y border-neutral-200 dark:border-neutral-800"
                    }`}
                  >
                    <h2 className="text-[13px] font-semibold tracking-wide text-neutral-700 tabular-nums dark:text-neutral-200">
                      {group.date}
                    </h2>
                    <span className="text-[11px] font-medium tracking-wider text-neutral-400 uppercase tabular-nums dark:text-neutral-500">
                      {group.items.length}{" "}
                      {group.items.length === 1 ? "set" : "sets"}
                    </span>
                  </div>

                  <ul className="divide-y divide-neutral-100 dark:divide-neutral-800">
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
          <div className="rounded-md border border-neutral-200 bg-white dark:border-neutral-800 dark:bg-neutral-900">
            <div className="border-b border-neutral-200 px-5 py-4 dark:border-neutral-800">
              <h2 className="text-[15px] font-semibold text-neutral-900 dark:text-neutral-50">
                Create performance
              </h2>
              <p className="mt-1 text-[13px] text-neutral-500 dark:text-neutral-400">
                Submitted in UTC, shown in Europe/Tallinn.
              </p>
            </div>

            <form onSubmit={formCreateHandler} className="space-y-4 px-5 py-5">
              <div className="space-y-2">
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

              <div className="space-y-2">
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
                <div className="space-y-2">
                  <label className={labelClassName} htmlFor="start-time">
                    Start time
                  </label>
                  <input
                    type="datetime-local"
                    id="start-time"
                    name="startTime"
                    value={startTime}
                    onChange={(e) => setStartTime(e.target.value)}
                    className={inputClassName}
                  />
                </div>

                <div className="space-y-2">
                  <label className={labelClassName} htmlFor="end-time">
                    End time
                  </label>
                  <input
                    type="datetime-local"
                    id="end-time"
                    name="endTime"
                    value={endTime}
                    onChange={(e) => setEndTime(e.target.value)}
                    className={inputClassName}
                  />
                </div>
              </div>

              <div className="space-y-2.5 border-t border-neutral-200 pt-4 dark:border-neutral-800">
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
                  <StatusAlert tone="success">
                    Performance created!
                  </StatusAlert>
                )}

                <button
                  type="submit"
                  className="inline-flex h-10 w-full items-center justify-center gap-2 rounded bg-accent-600 px-3 text-[15px] font-medium text-white transition-colors hover:bg-accent-700 focus-visible:ring-2 focus-visible:ring-accent-600 focus-visible:ring-offset-2 focus-visible:outline-none dark:bg-accent-500 dark:text-neutral-950 dark:hover:bg-accent-400 dark:focus-visible:ring-accent-400 dark:focus-visible:ring-offset-neutral-900"
                >
                  {mutation.isPending && (
                    <span>
                      <Spinner />
                    </span>
                  )}
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