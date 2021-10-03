#!/bin/bash

set -e
run_cmd="dotnet Ozon.DotNetCourse.SupplyService.dll --no-build -v d"
export PATH="$PATH:/root/.dotnet/tools"

dotnet Ozon.DotNetCourse.SupplyService.Migrator.dll --no-build -v d

>&2 echo "SupplyService DB Migrations complete, starting app."
>&2 echo "Running SupplyService': $run_cmd"
exec $run_cmd