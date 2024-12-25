export default function Header() {
  return (
    <header className="border-b border-b-slate-400">
      <div>
        <button className="rounded-md hover:bg-neutral-800 font-light text-sm p-1">
          Save
        </button>
        <button className="rounded-md hover:bg-neutral-800 font-light text-sm p-1">
          Upload
        </button>
        <button className="rounded-md hover:bg-neutral-800 font-light text-sm p-1">
          Run
        </button>
      </div>
    </header>
  );
}
