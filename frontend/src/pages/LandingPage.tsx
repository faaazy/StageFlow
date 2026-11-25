import { Clock, Mic, Ticket, type LucideIcon } from "lucide-react";
import { Link } from "react-router-dom";

const chipClassName =
  "inline-flex items-center justify-center rounded-[4px] border border-border px-2 py-[2px] text-[13px] font-medium text-text";

const primaryButtonClassName =
  "inline-flex h-11 w-auto items-center justify-center rounded-[4px] bg-accent px-6 text-[15px] font-semibold text-accent-fg transition-[filter] hover:brightness-95 focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none";

const secondaryButtonClassName =
  "inline-flex h-11 items-center justify-center rounded-[4px] border border-border px-6 text-[14px] text-text transition-colors hover:bg-surface focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none";

const organizerButtonClassName =
  "inline-flex h-11 items-center justify-center rounded-[2px] bg-bg px-6 text-[14px] font-semibold text-text transition-[filter] hover:brightness-110 focus-visible:ring-2 focus-visible:ring-bg focus-visible:ring-offset-2 focus-visible:ring-offset-accent focus-visible:outline-none";

const cardIconProps = {
  "aria-hidden": true,
  strokeWidth: 1.5,
  className: "size-5 shrink-0 text-accent",
};

const features: {
  icon: LucideIcon;
  title: string;
  description: string;
  comingSoon?: boolean;
}[] = [
  {
    icon: Clock,
    title: "Full running order",
    description:
      "Every set by stage and time, shown in the festival’s own time zone.",
  },
  {
    icon: Mic,
    title: "Stages and artists",
    description: "See who plays on which stage, with no overlaps and no guesswork.",
  },
  {
    icon: Ticket,
    title: "Tickets",
    description: "Buy tickets and keep them in one place, with a QR code for entry.",
    comingSoon: true,
  },
];

const lineup: {
  time: string;
  name: string;
  stage: string;
  size: string;
  accent?: boolean;
}[] = [
  {
    time: "18:00 – 18:45",
    name: "Artist One",
    stage: "Main Stage",
    size: "text-[clamp(0.875rem,4vw,1.75rem)]",
  },
  {
    time: "19:30 – 20:20",
    name: "Artist Two",
    stage: "Tent",
    size: "text-[clamp(1.125rem,5vw,2.25rem)]",
  },
  {
    time: "20:40 – 21:30",
    name: "Artist Three",
    stage: "Tent",
    size: "text-[clamp(1.5rem,6vw,3rem)]",
  },
  {
    time: "21:30 – 22:30",
    name: "Headliner",
    stage: "Main Stage",
    size: "text-[clamp(2rem,8vw,4rem)]",
    accent: true,
  },
];

const marqueeLine = "Set times · Stages · Artists · No overlaps · Tickets soon · ";
const marqueeHalf = marqueeLine.repeat(3);

export function LandingPage() {
  return (
    <>
      <div className="mx-auto w-full max-w-[72rem] px-4 pt-10 sm:px-6 lg:px-10 lg:pt-16">
        <div className="flex justify-between border-b border-border py-3 font-mono text-[12px] uppercase tracking-wider text-muted">
          <span>Festival operations &amp; ticketing</span>
          <span className="hidden sm:inline">
            Running order · Stages · Set times
          </span>
        </div>

        <section aria-labelledby="hero-title" className="pt-10 lg:pt-16">
          <h1
            id="hero-title"
            className="font-display text-[clamp(2.25rem,10.5vw,10rem)] font-black uppercase leading-[0.9] tracking-tight text-balance"
          >
            <span className="block">Every set.</span>{" "}
            <span className="block">Every stage.</span>{" "}
            <span className="block text-accent">On time.</span>
          </h1>

          <p className="mt-8 max-w-[38rem] text-[18px] leading-relaxed text-muted">
            StageFlow brings the whole festival schedule into one place: stages,
            artists and set times, checked so that nothing overlaps.
          </p>

          <div className="mt-8 flex flex-wrap items-center gap-3">
            <Link to="/register" className={primaryButtonClassName}>
              Create account
            </Link>
            <Link to="/login" className={secondaryButtonClassName}>
              Sign in
            </Link>
          </div>

          <div className="mt-4 flex flex-wrap items-center gap-2 text-[13px] text-muted">
            <span className={chipClassName}>Coming soon</span>
            <span>Public programs and ticket sales</span>
          </div>
        </section>

        <section aria-label="Sample running order" className="mt-14">
          <div className="rounded-[2px] border border-border bg-surface">
            <div className="flex justify-between border-b border-border bg-bg px-5 py-3">
              <p className="font-mono text-[12px] uppercase tracking-wider text-muted">
                Sample running order
              </p>
              <span className={chipClassName}>Sample data</span>
            </div>

            <div className="divide-y divide-border">
              {lineup.map(({ time, name, stage, size, accent }) => (
                <div
                  key={time}
                  className="grid grid-cols-1 gap-2 px-5 py-4 sm:grid-cols-[7.5rem_1fr_auto] sm:items-center sm:gap-4"
                >
                  <p className="font-mono text-[15px] tabular-nums text-muted">
                    {time}
                  </p>
                  <p
                    className={`font-display font-black uppercase leading-[0.9] ${size} ${accent ? "text-accent" : "text-text"}`}
                  >
                    {name}
                  </p>
                  <span
                    className={`${chipClassName} justify-self-start sm:justify-self-end`}
                  >
                    {stage}
                  </span>
                </div>
              ))}
            </div>

            <div
              aria-hidden="true"
              className="border-t border-border border-l-2 border-l-conflict bg-surface px-[14px] py-3"
            >
              <p className="font-semibold text-conflict">Schedule conflict</p>
              <p className="mt-0.5 break-words text-[15px] text-text">
                This performance overlaps another set on the same stage.
              </p>
            </div>
          </div>
        </section>
      </div>

      <div
        aria-hidden="true"
        className="mt-16 overflow-hidden border-y border-border bg-accent py-3 text-accent-fg"
      >
        <div className="flex w-max animate-marquee">
          {[0, 1].map((copy) => (
            <span
              key={copy}
              className="shrink-0 whitespace-nowrap pr-8 font-mono text-[14px] font-semibold uppercase tracking-wider"
            >
              {marqueeHalf}
            </span>
          ))}
        </div>
      </div>

      <div className="mx-auto w-full max-w-[72rem] px-4 sm:px-6 lg:px-10">
        <section aria-labelledby="features-title" className="mt-16">
          <h2 id="features-title" className="mb-6 text-[24px] font-semibold">
            What you get
          </h2>
          <div className="grid gap-4 md:grid-cols-3">
            {features.map(({ icon: Icon, title, description, comingSoon }, index) => (
              <article
                key={title}
                className="rounded-[2px] border border-border bg-surface p-5"
              >
                <div className="flex items-center justify-between">
                  <span className="font-mono text-[12px] font-medium text-accent">
                    0{index + 1}
                  </span>
                </div>

                <div
                  aria-hidden="true"
                  className="ticket-cutouts my-4 border-t border-dashed border-border"
                />

                <Icon {...cardIconProps} />
                <div className="mt-4 flex items-center justify-between gap-2">
                  <h3 className="text-[17px] font-semibold">{title}</h3>
                  {comingSoon && (
                    <span className={chipClassName}>Coming soon</span>
                  )}
                </div>
                <p className="mt-2 text-[15px] leading-relaxed text-muted">
                  {description}
                </p>
              </article>
            ))}
          </div>
        </section>

        <section
          aria-labelledby="organizers-title"
          className="mt-16 pb-20"
        >
          <div className="rounded-[2px] bg-accent p-6 text-accent-fg sm:p-10">
            <p className="font-mono text-[12px] uppercase tracking-wider">
              For organizers
            </p>
            <h2
              id="organizers-title"
              className="mt-4 font-display text-[clamp(2.25rem,7vw,4.5rem)] font-black uppercase leading-[0.95] tracking-tight text-balance text-accent-fg"
            >
              Organizing a festival?
            </h2>
            <p className="mt-4 max-w-[34rem] text-[17px] leading-relaxed text-accent-fg">
              Create festivals, add stages and artists, and build a schedule that
              never double-books a stage.
            </p>
            <Link to="/organizer" className={`${organizerButtonClassName} mt-6`}>
              For organizers
            </Link>
          </div>
        </section>
      </div>
    </>
  );
}
