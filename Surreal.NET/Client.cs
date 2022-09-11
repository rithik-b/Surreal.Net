﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Surreal.NET;

public interface ISurrealClient
{
    /// <summary>
    /// Opens the connection to a SurrealDB instance using the provided configuration.
    /// </summary>
    public Task Open(SurrealConfig config, CancellationToken ct = default);

    /// <summary>
    /// Returns a copy of the current configuration.
    /// </summary>
    public SurrealConfig GetConfig();

    /// <summary>
    /// Closes the open connection the the SurrealDB.
    /// </summary>
    public void Close();

    /// <summary>
    /// Switch to a specific namespace and database.
    /// </summary>
    /// <param name="db">Switches to a specific namespace.</param>
    /// <param name="ns">Switches to a specific database.</param>
    public Task Use(string db, string ns, CancellationToken ct = default);

    /// <summary>
    /// Signs up to a specific authentication scope.
    /// </summary>
    /// <param name="auth">Variables used in a signin query.</param>
    public Task Signup(string auth, CancellationToken ct = default);

    /// <summary>
    /// Signs in to a specific authentication scope.
    /// </summary>
    /// <param name="auth">Variables used in a signin query.</param>
    public Task Signin(string auth, CancellationToken ct = default);

    /// <summary>
    /// Invalidates the authentication for the current connection.
    /// </summary>
    public Task Invalidate(CancellationToken ct = default);

    /// <summary>
    /// Authenticates the current connection with a JWT token.
    /// </summary>
    /// <param name="token"> The JWT authentication token.</param>
    public Task Authenticate(string token, CancellationToken ct = default);

    /// <summary>
    /// Assigns a value as a parameter for this connection.
    /// </summary>
    /// <param name="key">Specifies the name of the variable.</param>
    /// <param name="value">Assigns the value to the variable name.</param>
    public Task Let(string key, string value, CancellationToken ct = default);

    /// <summary>
    /// Runs a set of SurrealQL statements against the database.
    /// </summary>#
    /// <param name="sql">Specifies the SurrealQL statements.</param>
    /// <param name="vars">Assigns variables which can be used in the query.</param>
    public Task Query(string sql, string vars, CancellationToken ct = default);

    /// <summary>
    /// Selects all records in a table, or a specific record, from the database.
    /// </summary>
    /// <param name="thing"> The table name or a record id to select.</param>
    /// <remarks>
    /// This function will run the following query in the database:
    /// <code>SELECT * FROM $thing;</code>
    /// </remarks>
    public Task Select(string thing, CancellationToken ct = default);

    /// <summary>
    /// Creates a record in the database.
    /// </summary>
    /// <param name="thing"> The table name or the specific record id to create. </param>
    /// <param name="data"> The document / record data to insert. </param>
    /// <remarks>
    /// This function will run the following query in the database:
    /// <code>CREATE $thing CONTENT $data;</code>
    /// </remarks>
    public Task Create(string thing, string data, CancellationToken ct = default);

    /// <summary>
    /// Updates all records in a table, or a specific record, in the database.
    /// </summary>
    /// <param name="thing"> The table name or the specific record id to update. </param>
    /// <param name="data"> The document / record data to insert. </param>
    /// <remarks>
    /// This function replaces the current document / record data with the specified data.
    ///
    /// This function will run the following query in the database:
    /// <code>UPDATE $thing CONTENT $data;</code>
    /// </remarks>
    public Task Update(string thing, string data, CancellationToken ct = default);

    /// <summary>
    /// Modifies all records in a table, or a specific record, in the database.
    /// </summary>
    /// <param name="thing"> The table name or the specific record id to update. </param>
    /// <param name="data"> The document / record data to insert. </param>
    /// <remarks>
    /// This function merges the current document / record data with the specified data.
    /// 
    /// This function will run the following query in the database:
    /// <code>UPDATE $thing MERGE $data;</code>
    /// </remarks>
    public Task Change(string thing, string data, CancellationToken ct = default);

    /// <summary>
    /// Applies  <see href="https://jsonpatch.com/">JSON Patch</see> changes to all records, or a specific record, in the database.
    /// </summary>
    /// <param name="thing"> The table name or the specific record id to update. </param>
    /// <param name="data"> The JSON Patch data with which to modify the records. </param>
    /// <remarks>
    /// This function patches the current document / record data with the specified JSON Patch data.
    ///
    /// This function will run the following query in the database:
    /// <code>UPDATE $thing PATCH $data;</code>
    /// </remarks>
    public Task Modify(string thing, string data, CancellationToken ct = default);

    /// <summary>
    /// Deletes all records in a table, or a specific record, from the database.
    /// </summary>
    /// <param name="thing"> The table name or a record id to select. </param>
    /// <remarks>
    /// This function will run the following query in the database:
    /// <code>DELETE * FROM $thing;</code>
    /// </remarks>
    public Task Delete(string thing, CancellationToken ct = default);
}