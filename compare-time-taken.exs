xsd_path = Path.join(__DIR__, "data/xsd/netex-v2.0.0/xsd/NeTEx_publication.xsd")
xml_folder = Path.join(__DIR__, "data/netex/fr-nap-idfm.zip.unpack")

log_time_taken = fn(message, cb) ->
  {delay, result} = :timer.tc(cb)
  IO.puts("#{message} took #{delay / 1_000_000.0} seconds")
  result
end

run_shell_command = fn(command) ->
  IO.write("Running #{command} ")
  {output, exit_code} = System.shell(command)
  IO.puts "\n#{output |> String.split("\n") |> Enum.count} lines returned in output, exit code is #{exit_code}"
end

log_time_taken.("fsx", fn() ->
  run_shell_command.("dotnet fsi xsd-validate.fsx #{xsd_path} #{xml_folder}")
end)

log_time_taken.("xmllint", fn() ->
  files = Path.wildcard("#{xml_folder}/**/*.xml") |> Enum.join(" ")
  run_shell_command.("xmllint --schema #{xsd_path} --noout #{files} 2>&1")
end)
