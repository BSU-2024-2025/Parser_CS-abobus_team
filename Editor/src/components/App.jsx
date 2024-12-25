import { useState } from "react";
import Header from "./Header";

function App() {
  const [code, setCode] = useState("// Your code here");

  const extractVariables = (code) => {
    const variableRegex = /(\w+)\s*=\s*(".*?"|\d+)/g;
    const variables = {};
    let match;
    while ((match = variableRegex.exec(code)) !== null) {
      variables[match[1]] = match[2];
    }
    return variables;
  };

  const variables = extractVariables(code);

  return (
    <div className="bg-neutral-900 h-screen text-white">
      <Header />
      <div className="h-4/6 flex">
        <textarea
          className="bg-neutral-800 h-full w-11/12 resize-none block focus:outline-none p-3"
          value={code}
          onChange={(e) => setCode(e.target.value)}
        />
        <div className="border-l border-l-slate-400 p-3 w-1/3">
          <h2 className="text-lg font-bold">Variables</h2>
          <ul>
            {Object.entries(variables).map(([key, value]) => (
              <li key={key}>
                {key}: {value}
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="border-t border-t-slate-400">
        <h2 className="p-2">Console</h2>
      </div>
    </div>
  );
}

export default App;
