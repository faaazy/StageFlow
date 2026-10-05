export function formatTime(value: string): string {
  const date = new Date(value);

  const formatter = new Intl.DateTimeFormat("en-GB", {
    hour: "2-digit",
    minute: "2-digit",
    timeZone: "Europe/Tallinn",
  });

  return formatter.format(date);
}

export function formatDate(value: string): string {
  const date = new Date(value);

  const formatter = new Intl.DateTimeFormat("en-GB", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    timeZone: "Europe/Tallinn",
  });

  return formatter.format(date).replace(/\//g, ".");
}
