import { Link, Outlet } from "react-router-dom";

export function AppLayout() {
  return (
    <div>
      <header>
        <div className="logo">StageFlow</div>
        <ul>
          <li>
            <Link to={"schedule"}>Schedule</Link>
          </li>
        </ul>
      </header>

      <main>
        <Outlet />
      </main>

      <footer>credits</footer>
    </div>
  );
}
